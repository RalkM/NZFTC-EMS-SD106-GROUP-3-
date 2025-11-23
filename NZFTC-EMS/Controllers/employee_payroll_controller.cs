using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;


namespace NZFTC_EMS.Controllers
{
    [Route("employee_payroll")]
    public class employee_payroll_controller : Controller
    {
        private readonly AppDbContext _context;

        public employee_payroll_controller(AppDbContext context)
        {
            _context = context;
        }

        private int? GetEmployeeId()
        {
            return HttpContext.Session.GetInt32("EmployeeId");
        }

        private bool IsEmployee()
        {
            return HttpContext.Session.GetString("Role") == "Employee";
        }

        // =========================
        // PAYSLIP HISTORY LIST
        // =========================
        [HttpGet("")]
        [HttpGet("payslips")]
        public async Task<IActionResult> Payslips()
        {
            var eid = GetEmployeeId();

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var slips = await _context.EmployeePayrollSummaries
                .Include(p => p.PayrollRun)
                .Where(p => p.EmployeeId == eid.Value)
                .OrderByDescending(p => p.GeneratedAt)
                .ToListAsync();

            return View(
                "~/Views/website/employee/payslips.cshtml",
                slips
            );
        }

        // GET: /employee_payroll/payslip/5
[HttpGet("payslip/{id:int}")]
public async Task<IActionResult> Payslip(int id)
{
    var eid = HttpContext.Session.GetInt32("EmployeeId");
    if (eid == null)
        return RedirectToAction("Login", "Website");

    var payslip = await _context.EmployeePayrollSummaries
        .Include(p => p.Employee)
        .Include(p => p.PayrollRun)
        .FirstOrDefaultAsync(p => p.EmployeePayrollSummaryId == id && p.EmployeeId == eid.Value);

    if (payslip == null)
        return NotFound(); // or RedirectToAction("Payslips")

    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    return View(
        "~/Views/website/employee/payslip_details.cshtml",
        payslip
    );
}

    }
}
