using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities; // <-- entities & enums
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        ? $"{DateTime.UtcNow:yyyy-MM}-M1"
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
        public async Task<IActionResult> Payslip(string name)
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            if (string.IsNullOrWhiteSpace(name))
                name = "John Doe";

            var names = name.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var first = names.Length > 0 ? names[0] : name;
            var last = names.Length > 1 ? names[1] : "";

            var rows = await _context.EmployeePayrollSummaries
                .Include(s => s.Employee)
                .Include(s => s.PayrollPeriod)
                .Where(s => s.Employee.FirstName == first && s.Employee.LastName == last)
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

        [HttpGet("seed-test")]
        public async Task<IActionResult> SeedTest()
        {
            // Seed a couple of employees if missing
            if (!await _context.Employees.AnyAsync())
            {
                _context.Employees.AddRange(
                    new Employee { FirstName = "John", LastName = "Doe", Email = "john@ems.local", StartDate = DateTime.Today.AddYears(-1) },
                    new Employee { FirstName = "Jane", LastName = "Smith", Email = "jane@ems.local", StartDate = DateTime.Today.AddYears(-2) },
                    new Employee { FirstName = "Liam", LastName = "Brown", Email = "liam@ems.local", StartDate = DateTime.Today.AddYears(-3) }
                );
                await _context.SaveChangesAsync();
            }

            var period = await _context.PayrollPeriods.FirstOrDefaultAsync(p => p.PeriodCode == "2025-11-M1");
            if (period == null)
            {
                period = new PayrollPeriod
                {
                    PeriodCode = "2025-11-M1",
                    PeriodStart = new DateTime(2025, 11, 1),
                    PeriodEnd = new DateTime(2025, 11, 30),
                    TotalAmount = 0m,
                    Closed = false
                };
                _context.PayrollPeriods.Add(period);
                await _context.SaveChangesAsync();
            }

            var john = await _context.Employees.FirstAsync(e => e.FirstName == "John" && e.LastName == "Doe");
            var jane = await _context.Employees.FirstAsync(e => e.FirstName == "Jane" && e.LastName == "Smith");
            var liam = await _context.Employees.FirstAsync(e => e.FirstName == "Liam" && e.LastName == "Brown");

            void Upsert(int empId, decimal rate, decimal gross, decimal ded)
            {
                var existing = _context.EmployeePayrollSummaries
                    .FirstOrDefault(s => s.PayrollPeriodId == period.PayrollPeriodId && s.EmployeeId == empId);

                if (existing == null)
                {
                    _context.EmployeePayrollSummaries.Add(new EmployeePayrollSummary
                    {
                        PayrollPeriodId = period.PayrollPeriodId,
                        EmployeeId = empId,
                        PayRate = rate,
                        GrossPay = gross,
                        Deductions = ded,
                        Status = PayrollSummaryStatus.Finalized
                    });
                }
                else
                {
                    existing.PayRate = rate;
                    existing.GrossPay = gross;
                    existing.Deductions = ded;
                    existing.Status = PayrollSummaryStatus.Finalized;
                }
            }

            Upsert(john.EmployeeId, 35m, 4800m, 200m);
            Upsert(jane.EmployeeId, 38m, 5200m, 150m);
            Upsert(liam.EmployeeId, 42m, 6000m, 400m);

            await _context.SaveChangesAsync();
            TempData["msg"] = "Seeded payroll summaries.";
            return RedirectToAction("Index");
        }
    }
}
