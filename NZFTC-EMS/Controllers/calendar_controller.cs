using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using NZFTC_EMS.Models.ViewModels;

namespace NZFTC_EMS.Controllers
{
    [Route("calendar")]
    public class calendar_controller : Controller
    {
        private readonly AppDbContext _context;

        public calendar_controller(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";

        private string? CurrentUsername => HttpContext.Session.GetString("Username");

        // GET /calendar or /calendar/UserCalendar?month=11&year=2025
        [HttpGet("")]
        [HttpGet("UserCalendar")]
        public async Task<IActionResult> UserCalendar(int? month, int? year)
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            ViewData["Title"] = "Calendar";

            var isAdmin = IsAdmin();

            var today = DateTime.Today;
            int targetMonth = month ?? today.Month;
            int targetYear = year ?? today.Year;

            IQueryable<CalendarEvent> query = _context.CalendarEvents;

            // Phase 2 rules (simple):
            // Admin: see all events
            // Employee: see their own events + global events (OwnerUsername == null)
            if (!isAdmin)
            {
                var username = CurrentUsername;
                query = query.Where(e =>
                    e.OwnerUsername == null ||
                    (username != null && e.OwnerUsername == username));
            }

            // Only events in the target month/year
            query = query.Where(e => e.Start.Month == targetMonth && e.Start.Year == targetYear);

            var events = await query
                .OrderBy(e => e.Start)
                .ToListAsync();

            var vm = new CalendarVm
            {
                Events = events,
                CurrentMonth = targetMonth,
                CurrentYear = targetYear,
                IsAdmin = isAdmin
            };

            if (isAdmin)
            {
                return View("~/Views/website/admin/calendar_user.cshtml", vm);
            }

            return View("~/Views/website/employee/calendar_user.cshtml", vm);
        }
    }
}


