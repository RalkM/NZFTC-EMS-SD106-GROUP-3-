using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Controllers
{
    [Route("employee_timesheets")]
    public class employee_timesheet_controller : Controller
    {
        private readonly AppDbContext _context;

        public employee_timesheet_controller(AppDbContext context)
        {
            _context = context;
        }

        private int? GetEmployeeId()
            => HttpContext.Session.GetInt32("EmployeeId");

        private bool IsEmployee()
            => HttpContext.Session.GetString("Role") == "Employee";

        // ================= LIST =================
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index(DateTime? from, DateTime? to)
        {
            var eid = GetEmployeeId();

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var today = DateTime.Today;
            from ??= today.AddDays(-6);
            to ??= today;

            var fromDate = from.Value.Date;
            var toDate   = to.Value.Date;

            var entries = await _context.TimesheetEntries
                .Where(t => t.EmployeeId == eid.Value
                            && t.WorkDate >= fromDate
                            && t.WorkDate <= toDate)
                .OrderByDescending(t => t.WorkDate)
                .ThenBy(t => t.StartTime)
                .ToListAsync();

            ViewBag.From = fromDate.ToString("yyyy-MM-dd");
            ViewBag.To   = toDate.ToString("yyyy-MM-dd");

            return View(
                "~/Views/website/employee/timesheets.cshtml",
                entries
            );
        }

        // ================= SUBMIT =================
        [HttpPost("submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(TimesheetEntry model)
        {
            var eid = GetEmployeeId();

            if (model.WorkDate == default)
                ModelState.AddModelError("WorkDate", "Work date is required.");

            if (!ModelState.IsValid)
            {
                // reload list on validation fail
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

                var today = DateTime.Today;
                var from = today.AddDays(-6);
                var to = today;

                var entries = await _context.TimesheetEntries
                    .Where(t => t.EmployeeId == eid.Value
                                && t.WorkDate >= from
                                && t.WorkDate <= to)
                    .OrderByDescending(t => t.WorkDate)
                    .ThenBy(t => t.StartTime)
                    .ToListAsync();

                ViewBag.From = from.ToString("yyyy-MM-dd");
                ViewBag.To   = to.ToString("yyyy-MM-dd");

                return View(
                    "~/Views/website/employee/timesheets.cshtml",
                    entries
                );
            }

            model.EmployeeId   = eid.Value;
            model.RecalculateTotalHours();
            model.Status       = TimesheetStatus.Submitted;
            model.CreatedAt    = DateTime.UtcNow;
            model.SubmittedAt  = DateTime.UtcNow;

            _context.TimesheetEntries.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Timesheet submitted.";
            return RedirectToAction("Index");
        }
    }
}
