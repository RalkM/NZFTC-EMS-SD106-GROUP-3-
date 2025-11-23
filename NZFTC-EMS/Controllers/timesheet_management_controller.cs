using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Controllers
{
    [Route("timesheet_management")]
    public class timesheet_management_controller : Controller
    {
        private readonly AppDbContext _context;

        public timesheet_management_controller(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("Role") == "Admin";

        // ============================================================
        // LIST TIMESHEETS (admin view)
        // ============================================================
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index(DateTime? from, DateTime? to)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var today = DateTime.Today;
            from ??= today.AddDays(-6);
            to ??= today;

            var fromDate = from.Value.Date;
            var toDate   = to.Value.Date;

            var entries = await _context.TimesheetEntries
                .Include(t => t.Employee)
                .Where(t => t.WorkDate >= fromDate && t.WorkDate <= toDate)
                .OrderByDescending(t => t.WorkDate)
                .ThenBy(t => t.Employee.LastName)
                .ToListAsync();

            ViewBag.From = fromDate.ToString("yyyy-MM-dd");
            ViewBag.To   = toDate.ToString("yyyy-MM-dd");

            return View(
                "~/Views/website/admin/timesheets.cshtml",
                entries
            );
        }

        // ============================================================
        // APPROVE
        // ============================================================
        [HttpPost("approve/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            var entry = await _context.TimesheetEntries.FindAsync(id);
            if (entry == null)
            {
                TempData["Error"] = "Timesheet entry not found.";
                return RedirectToAction("Index");
            }

            entry.Status = TimesheetStatus.Approved;
            entry.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Timesheet approved.";
            return RedirectToAction("Index");
        }

        // ============================================================
        // REJECT
        // ============================================================
        [HttpPost("reject/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string? note)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "website");

            var entry = await _context.TimesheetEntries.FindAsync(id);
            if (entry == null)
            {
                TempData["Error"] = "Timesheet entry not found.";
                return RedirectToAction("Index");
            }

            entry.Status = TimesheetStatus.Rejected;
            entry.AdminNote = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
            entry.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Timesheet rejected.";
            return RedirectToAction("Index");
        }
    }
}
