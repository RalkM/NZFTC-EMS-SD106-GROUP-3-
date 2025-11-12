using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities; // <-- entities & enums
using System.Text;

namespace NZFTC_EMS.Controllers
{
    // VMs for the views
    public class PayrollRowVm
    {
        public int Id { get; set; }                    // EmployeePayrollSummaryId
        public string EmployeeName { get; set; } = "";
        public string PeriodCode { get; set; } = "";
        public decimal PayRate { get; set; }
        public decimal Gross { get; set; }
        public decimal Deductions { get; set; }
        public decimal Net { get; set; }
        public string Status { get; set; } = "";
    }

    public class PayrollInputVm
    {
        public string EmployeeName { get; set; } = ""; // "First Last"
        public string PeriodCode { get; set; } = "";   // e.g. "2025-11-M1"
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal PayRate { get; set; }
        public decimal Gross { get; set; }
        public decimal Deductions { get; set; }
    }

    [Route("payroll_management")]
    public class payroll_management_controller : Controller
    {
        private readonly AppDbContext _context;
        public payroll_management_controller(AppDbContext context) => _context = context;

        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var rows = await _context.EmployeePayrollSummaries
                .Include(s => s.Employee)
                .Include(s => s.PayrollPeriod)
                .OrderByDescending(s => s.PayrollPeriod.PeriodStart)
                .Select(s => new PayrollRowVm
                {
                    Id = s.EmployeePayrollSummaryId,
                    EmployeeName = s.Employee.FirstName + " " + s.Employee.LastName,
                    PeriodCode = s.PayrollPeriod.PeriodCode,
                    PayRate = s.PayRate,
                    Gross = s.GrossPay,
                    Deductions = s.Deductions,
                    Net = s.NetPay,
                    Status = s.Status.ToString()
                })
                .ToListAsync();

            return View("~/Views/website/admin/payroll_management.cshtml", rows);
        }

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PayrollInputVm model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            // find employee by name
            var names = (model.EmployeeName ?? "").Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var first = names.Length > 0 ? names[0] : model.EmployeeName;
            var last = names.Length > 1 ? names[1] : "";

            var emp = await _context.Employees
                .FirstOrDefaultAsync(e => e.FirstName == first && e.LastName == last);

            if (emp == null)
            {
                TempData["msg"] = $"Employee '{model.EmployeeName}' not found.";
                return RedirectToAction("Index");
            }

            // get or create payroll period
            var period = await _context.PayrollPeriods
                .FirstOrDefaultAsync(p => p.PeriodCode == model.PeriodCode);

            if (period == null)
            {
                period = new PayrollPeriod
                {
                    PeriodCode = string.IsNullOrWhiteSpace(model.PeriodCode)
                        ? $"{DateTime.UtcNow:yyyy-MM}"
                        : model.PeriodCode,
                    PeriodStart = model.PeriodStart == default ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1) : model.PeriodStart,
                    PeriodEnd = model.PeriodEnd == default ? DateTime.Today : model.PeriodEnd,
                    TotalAmount = 0m,
                    Closed = false
                };
                _context.PayrollPeriods.Add(period);
                await _context.SaveChangesAsync();
            }

            // upsert (one summary per period+employee)
            var existing = await _context.EmployeePayrollSummaries
                .FirstOrDefaultAsync(s => s.PayrollPeriodId == period.PayrollPeriodId && s.EmployeeId == emp.EmployeeId);

            if (existing == null)
            {
                var s = new EmployeePayrollSummary
                {
                    PayrollPeriodId = period.PayrollPeriodId,
                    EmployeeId = emp.EmployeeId,
                    PayRate = model.PayRate,
                    GrossPay = model.Gross,
                    Deductions = model.Deductions,
                    Status = PayrollSummaryStatus.Finalized
                };
                _context.EmployeePayrollSummaries.Add(s);
            }
            else
            {
                existing.PayRate = model.PayRate;
                existing.GrossPay = model.Gross;
                existing.Deductions = model.Deductions;
                existing.Status = PayrollSummaryStatus.Finalized;
            }

            await _context.SaveChangesAsync();
            TempData["msg"] = "Payroll added/updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet("payslip")]
        public async Task<IActionResult> Payslip(string? name)
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            if (string.IsNullOrWhiteSpace(name))
                return View("~/Views/website/employee/payslip.cshtml", new List<PayrollRowVm>());

            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var first = parts.ElementAtOrDefault(0) ?? "";
            var last = string.Join(' ', parts.Skip(1));

            var rows = await _context.EmployeePayrollSummaries
                .AsNoTracking()
                .Include(s => s.Employee)
                .Include(s => s.PayrollPeriod)
                .Where(s => s.Employee.FirstName == first &&
                            (string.IsNullOrEmpty(last) || s.Employee.LastName == last))
                .OrderByDescending(s => s.PayrollPeriod.PeriodStart)
                .Select(s => new PayrollRowVm {   Id = s.EmployeePayrollSummaryId,
    EmployeeName = s.Employee.FirstName + " " + s.Employee.LastName,
    PeriodCode = s.PayrollPeriod.PeriodCode,
    PayRate = s.PayRate,
    Gross = s.GrossPay,
    Deductions = s.Deductions,
    Net = s.NetPay,
    Status = s.Status.ToString() })
                .ToListAsync();

            return View("~/Views/website/employee/payslip.cshtml", rows);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var s = await _context.EmployeePayrollSummaries
                .Include(x => x.Employee)
                .Include(x => x.PayrollPeriod)
                .FirstOrDefaultAsync(x => x.EmployeePayrollSummaryId == id);

            if (s == null)
                return NotFound();

            var csv = new StringBuilder();
            csv.AppendLine("Employee Name,Period,Pay Rate,Gross,Deductions,Net");
            csv.AppendLine($"{s.Employee.FirstName} {s.Employee.LastName},{s.PayrollPeriod.PeriodCode},{s.PayRate},{s.GrossPay},{s.Deductions},{s.NetPay}");

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv",
                $"Payslip_{s.Employee.FirstName}_{s.Employee.LastName}_{s.PayrollPeriod.PeriodCode}.csv");
        }
        #if DEBUG
[HttpPost("wipe-seed")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> WipeSeed()
{
    // Find the seeded employees
    var firstLast = new (string First, string Last)[] {
        ("John","Doe"), ("Jane","Smith"), ("Liam","Brown")
    };

    var seedEmployees = await _context.Employees
        .Where(e => firstLast.Any(n => e.FirstName == n.First && e.LastName == n.Last))
        .ToListAsync();

    if (seedEmployees.Count > 0)
    {
        var seedEmployeeIds = seedEmployees.Select(e => e.EmployeeId).ToList();

        // Delete summaries for those employees
        var summaries = await _context.EmployeePayrollSummaries
            .Where(s => seedEmployeeIds.Contains(s.EmployeeId))
            .ToListAsync();
        _context.EmployeePayrollSummaries.RemoveRange(summaries);
        await _context.SaveChangesAsync();

        // Remove any payroll periods that are now orphaned (no summaries)
        var unusedPeriods = await _context.PayrollPeriods
            .Where(p => !_context.EmployeePayrollSummaries.Any(s => s.PayrollPeriodId == p.PayrollPeriodId))
            .ToListAsync();
        _context.PayrollPeriods.RemoveRange(unusedPeriods);
        await _context.SaveChangesAsync();

        // Finally remove the sample employees
        _context.Employees.RemoveRange(seedEmployees);
        await _context.SaveChangesAsync();
    }

    TempData["msg"] = "Seed data wiped.";
    return RedirectToAction(nameof(Index));
}
#endif

    }
    
    
}
