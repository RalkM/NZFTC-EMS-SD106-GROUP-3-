using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using NZFTC_EMS.Models.ViewModels.Payroll;
using NZFTC_EMS.Services.Payroll;
using NZFTC_EMS.ViewModels.Payroll;

namespace NZFTC_EMS.Controllers
{
    public class PayrollManagementController : Controller
    {
        private readonly AppDbContext _db;
        private readonly PayrollService _payrollService;
        private readonly TaxService _taxService;
        private readonly PayslipService _payslipService;
        private readonly PayrollReportService _reportService;

        public PayrollManagementController(
            AppDbContext db,
            PayrollService payrollService,
            TaxService taxService,
            PayslipService payslipService,
            PayrollReportService reportService)
        {
            _db = db;
            _payrollService = payrollService;
            _taxService = taxService;
            _payslipService = payslipService;
            _reportService = reportService;
        }

        // ============================================================
        // NEW: INDEX ROUTE FOR /payroll_management
        // ============================================================
        public IActionResult Index()
        {
            return RedirectToAction("PayrollPeriods");
        }

        // ============================================================
        // ADMIN — PAYROLL PERIOD LIST
        // ============================================================
        public async Task<IActionResult> PayrollPeriods()
        {
            var periods = await _db.PayrollPeriods
                .OrderByDescending(x => x.PeriodStart)
                .Select(x => new PayrollPeriodVM
                {
                    PayrollPeriodId = x.PayrollPeriodId,
                    PeriodCode = x.PeriodCode,
                    PeriodStart = x.PeriodStart,
                    PeriodEnd = x.PeriodEnd,
                    Closed = x.Closed,
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync();

            return View("~/Views/website/admin/payroll_periods.cshtml", periods);
        }

        // ============================================================
        // ADMIN — CREATE PAYROLL PERIOD
        // ============================================================
        [HttpGet]
        public IActionResult CreatePeriod()
        {
            return View("~/Views/website/admin/create_payroll_period.cshtml", new PayrollPeriodVM());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePeriod(PayrollPeriodVM vm)
        {
            if (!ModelState.IsValid)
                return View("~/Views/website/admin/create_payroll_period.cshtml", vm);

            var period = new PayrollPeriod
            {
                PeriodCode = vm.PeriodCode,
                PeriodStart = vm.PeriodStart,
                PeriodEnd = vm.PeriodEnd,
                Closed = false
            };

            _db.PayrollPeriods.Add(period);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Payroll period created.";
            return RedirectToAction("PayrollPeriods");
        }

        // ============================================================
        // ADMIN — RUN PAYROLL
        // ============================================================
        public async Task<IActionResult> RunPayroll(int id)
        {
            var period = await _db.PayrollPeriods.FindAsync(id);
            if (period == null)
                return NotFound();

            if (period.Closed)
            {
                TempData["Error"] = "This payroll period is closed.";
                return RedirectToAction("PayrollPeriods");
            }

            await _payrollService.GeneratePayrollAsync(id);

            TempData["Success"] = "Payroll generated successfully.";
            return RedirectToAction("ViewSummary", new { id });
        }

        // ============================================================
        // ADMIN — SUMMARY
        // ============================================================
        public async Task<IActionResult> ViewSummary(int id)
        {
            var data = await _db.EmployeePayrollSummaries
                .Include(x => x.Employee)
                .Where(x => x.PayrollPeriodId == id)
                .ToListAsync();

            var vm = data.Select(x => new PayrollSummaryVM
            {
                EmployeePayrollSummaryId = x.EmployeePayrollSummaryId,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                PayRate = x.PayRate,
                RateType = x.Employee.PayGrade?.RateType.ToString() ?? "Unknown",

                GrossPay = x.GrossPay,
                PAYE = Math.Round(x.Deductions, 2),
                KiwiSaverEmployee = Math.Round(x.GrossPay * 0.03m, 2),
                KiwiSaverEmployer = Math.Round(x.GrossPay * 0.03m, 2),
                ACCLevy = Math.Round(x.GrossPay * 0.0153m, 2),
                StudentLoan = 0,

                NetPay = x.NetPay,
                Status = x.Status.ToString()
            }).ToList();

            return View("~/Views/website/admin/payroll_summary_admin.cshtml", vm);
        }

        // ============================================================
        // ADMIN — FINALIZE PAYROLL
        // ============================================================
        public async Task<IActionResult> FinalizePayroll(int id)
        {
            await _payrollService.FinalizePayrollAsync(id);

            TempData["Success"] = "Payroll finalized.";
            return RedirectToAction("ViewSummary", new { id });
        }

        // ============================================================
        // ADMIN — MARK AS PAID
        // ============================================================
        public async Task<IActionResult> MarkPaid(int id)
        {
            await _payrollService.MarkPayrollPaidAsync(id);

            TempData["Success"] = "Payroll marked as PAID.";
            return RedirectToAction("ViewSummary", new { id });
        }

        // ============================================================
        // ADMIN — SETTINGS
        // ============================================================
        [HttpGet]
        public IActionResult PayrollSettings()
        {
            var vm = new PayrollSettingsVM
            {
                KiwiSaverEmployeePercent = 3,
                KiwiSaverEmployerPercent = 3,
                ACCLevyPercent = 1.53m,
                EnableStudentLoan = true,
                RegularHoursPerWeek = 40,
                OvertimeMultiplier = 1.5m
            };

            return View("~/Views/website/admin/payroll_settings.cshtml", vm);
        }

        [HttpPost]
        public IActionResult PayrollSettings(PayrollSettingsVM vm)
        {
            TempData["Success"] = "Payroll settings saved.";
            return RedirectToAction("PayrollSettings");
        }

        // ============================================================
        // ADMIN — REPORTS
        // ============================================================
        public async Task<IActionResult> PayrollReports()
        {
            var vm = await _reportService.GeneratePayrollReportAsync();
            return View("~/Views/website/admin/payroll_reports.cshtml", vm);
        }

        // ============================================================
        // EMPLOYEE — PAYSLIPS LIST
        // ============================================================
        public async Task<IActionResult> Payslips()
        {
            int employeeId = GetEmployeeId();

            var payslips = await _db.EmployeePayrollSummaries
                .Include(x => x.PayrollPeriod)
                .Where(x => x.EmployeeId == employeeId &&
                            x.Status == PayrollSummaryStatus.Finalized)
                .OrderByDescending(x => x.PayrollPeriod.PeriodStart)
                .ToListAsync();

            return View("~/Views/website/employee/payslips.cshtml", payslips);
        }

        // ============================================================
        // EMPLOYEE — PAYSLIP DETAILS
        // ============================================================
        public async Task<IActionResult> PayslipDetails(int id)
        {
            var vm = await _payslipService.BuildPayslipAsync(id);
            return View("~/Views/website/employee/payslip_details.cshtml", vm);
        }

        // ============================================================
        // HELPER — GET EMPLOYEE ID
        // ============================================================
        private int GetEmployeeId()
        {
            if (HttpContext.Session.GetInt32("EmployeeId") is int id)
                return id;

            throw new Exception("Employee is not logged in.");
        }
    }
}
