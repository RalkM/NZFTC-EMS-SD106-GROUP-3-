using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using NZFTC_EMS.Services.Payroll;

namespace NZFTC_EMS.Controllers
{
    [Route("payroll_management")]
    public class payroll_management_controller : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPayrollService _payrollService;

        public payroll_management_controller(AppDbContext context, IPayrollService payrollService)
        {
            _context = context;
            _payrollService = payrollService;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        // =========================
        // DASHBOARD / SUMMARY
        // =========================
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var runs = await _context.PayrollRuns
                .Include(r => r.Payslips)
                .OrderByDescending(r => r.PeriodStart)
                .ToListAsync();

            // Admin overview – total runs, status, etc.
            return View(
                "~/Views/website/admin/payroll_summary_admin.cshtml",
                runs
            );
        }

        // =========================
        // PERIODS LIST (Thu–Wed)
        // =========================
        [HttpGet("periods")]
        public async Task<IActionResult> Periods()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var runs = await _context.PayrollRuns
                .Include(r => r.Payslips)
                .OrderByDescending(r => r.PeriodStart)
                .ToListAsync();

            return View(
                "~/Views/website/admin/payroll_periods.cshtml",
                runs
            );
        }

        // =========================
        // DETAILS – single run + payslips
        // =========================
        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var run = await _context.PayrollRuns
                .Include(r => r.Payslips)
                    .ThenInclude(p => p.Employee)
                .FirstOrDefaultAsync(r => r.PayrollRunId == id);

            if (run == null)
                return NotFound();

            return View(
                "~/Views/website/admin/payroll_details.cshtml",
                run
            );
        }

[HttpPost("create_weekly")]
[ValidateAntiForgeryToken]
public Task<IActionResult> CreateWeekly()
    => CreateAndCompute(PayFrequency.Weekly);

[HttpPost("create_fortnightly")]
[ValidateAntiForgeryToken]
public Task<IActionResult> CreateFortnightly()
    => CreateAndCompute(PayFrequency.Fortnightly);

[HttpPost("create_monthly")]
[ValidateAntiForgeryToken]
public Task<IActionResult> CreateMonthly()
    => CreateAndCompute(PayFrequency.Monthly);
    
        // =========================
        // CREATE + COMPUTE LATEST WEEKLY RUN
        // (Thu–Wed block, using PayrollService)
        // =========================

           private async Task<IActionResult> CreateAndCompute(PayFrequency freq)
{
    if (!IsAdmin())
        return RedirectToAction("AccessDenied", "website");

    var today = DateTime.Today;
    DateTime periodStart;
    DateTime periodEnd;

    // Decide period based on frequency
    switch (freq)
    {
        case PayFrequency.Weekly:
            {
                // Thu–Wed week, using last completed Wednesday
                int daysSinceWed = ((int)today.DayOfWeek - (int)DayOfWeek.Wednesday + 7) % 7;
                periodEnd = today.AddDays(-daysSinceWed).Date;   // Wednesday
                periodStart = periodEnd.AddDays(-6).Date;        // Previous Thursday
                break;
            }

        case PayFrequency.Fortnightly:
            {
                // 2-week block ending on last completed Wednesday
                int daysSinceWed = ((int)today.DayOfWeek - (int)DayOfWeek.Wednesday + 7) % 7;
                periodEnd = today.AddDays(-daysSinceWed).Date;   // Wednesday
                periodStart = periodEnd.AddDays(-13).Date;       // 14-day period
                break;
            }

        case PayFrequency.Monthly:
            {
                var firstOfMonth = new DateTime(today.Year, today.Month, 1);
                periodStart = firstOfMonth;
                periodEnd   = firstOfMonth.AddMonths(1).AddDays(-1);  // last day of month
                break;
            }

        default:
            // Fallback: treat unknown as weekly
            int d = ((int)today.DayOfWeek - (int)DayOfWeek.Wednesday + 7) % 7;
            periodEnd = today.AddDays(-d).Date;
            periodStart = periodEnd.AddDays(-6).Date;
            break;
    }

    // Check if we already have a run for this frequency & period
    var existing = await _context.PayrollRuns
        .FirstOrDefaultAsync(r =>
            r.PeriodStart  == periodStart &&
            r.PeriodEnd    == periodEnd &&
            r.PayFrequency == freq);

    PayrollRun run;
    if (existing == null)
    {
        // create a new run for THIS frequency
        run = await _payrollService.CreateRunAsync(
            periodStart,
            periodEnd,
            freq
        );
    }
    else
    {
        run = existing;
    }

    // Compute (or recompute) payslips for this run
    await _payrollService.ComputeRunAsync(run.PayrollRunId);

    TempData["Success"] =
        $"{freq} payroll created and computed for {periodStart:dd MMM} – {periodEnd:dd MMM}.";

    return RedirectToAction("Details", new { id = run.PayrollRunId });
}

            

        private (DateTime start, DateTime end) GetNextPeriod(PayFrequency freq)
{
    // base "today" on NZ time if you want; for now use UTC date
    var today = DateTime.UtcNow.Date;

    switch (freq)
    {
        case PayFrequency.Weekly:
            // Thu–Wed week
            // find most recent Thursday
            int daysSinceThu = ((int)today.DayOfWeek - (int)DayOfWeek.Thursday + 7) % 7;
            var weekEnd = today.AddDays(-daysSinceThu);   // this Wed or past Wed
            var weekStart = weekEnd.AddDays(-6);
            return (weekStart, weekEnd);

        case PayFrequency.Fortnightly:
            // 2-week block ending on Wednesday
            int d2 = ((int)today.DayOfWeek - (int)DayOfWeek.Thursday + 7) % 7;
            var fnEnd = today.AddDays(-d2);               // Wed
            var fnStart = fnEnd.AddDays(-13);             // 14-day period
            return (fnStart, fnEnd);

        case PayFrequency.Monthly:
        default:
            var firstOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastOfMonth  = firstOfMonth.AddMonths(1).AddDays(-1);
            return (firstOfMonth, lastOfMonth);
    }
}


        // =========================
        // RE-COMPUTE existing run
        // =========================
        [HttpPost("compute/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Compute(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            await _payrollService.ComputeRunAsync(id);

            TempData["Success"] = "Payroll run recomputed.";
            return RedirectToAction("Details", new { id });
        }

        // =========================
        // MARK RUN AS PAID
        // =========================
        [HttpPost("mark_paid/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkPaid(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            var ok = await _payrollService.MarkRunAsPaidAsync(id);

            if (!ok)
            {
                TempData["Error"] = "Unable to mark payroll run as paid.";
            }
            else
            {
                TempData["Success"] = "Payroll run marked as PAID.";
            }

            return RedirectToAction("Details", new { id });
        }

        // =========================
        // REPORTS
        // =========================
        [HttpGet("reports")]
        public async Task<IActionResult> Reports()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            // You can aggregate here as needed
            var runs = await _context.PayrollRuns
                .Include(r => r.Payslips)
                .OrderByDescending(r => r.PeriodStart)
                .ToListAsync();

            return View(
                "~/Views/website/admin/payroll_reports.cshtml",
                runs
            );
        }

        // =========================
        // SETTINGS
        // =========================
        [HttpGet("settings")]
        public async Task<IActionResult> Settings()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var settings = await _context.PayrollSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new PayrollSettings();
                _context.PayrollSettings.Add(settings);
                await _context.SaveChangesAsync();
            }

            return View(
                "~/Views/website/admin/payroll_settings.cshtml",
                settings
            );
        }

        [HttpPost("settings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(PayrollSettings model)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View(
                    "~/Views/website/admin/payroll_settings.cshtml",
                    model
                );
            }

            _context.PayrollSettings.Update(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Payroll settings updated.";
            return RedirectToAction("Settings");
        }
    }
}
