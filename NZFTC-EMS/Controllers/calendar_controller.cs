using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace NZFTC_EMS.Controllers
{
    // Simple ViewModel for calendar events (you can replace with your own later)
    public class CalendarEventVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public bool AllDay { get; set; }
        public string? Description { get; set; }
    }

    [Route("calendar")]
    public class calendar_controller : Controller
    {
        // TEMP: in-memory events (just so the view has something to bind to)
        // You can delete this later when you wire to the database.
        private static readonly List<CalendarEventVm> _sampleEvents = new()
        {
            new CalendarEventVm
            {
                Id = 1,
                Title = "Public Holiday",
                Start = DateTime.Today.AddDays(3),
                AllDay = true
            },
            new CalendarEventVm
            {
                Id = 2,
                Title = "Payroll Cutoff",
                Start = DateTime.Today.AddDays(7),
                AllDay = true
            }
        };

        // ==== ADMIN CALENDAR ====
        // URL: /calendar/admin
        [HttpGet("admin")]
        public IActionResult Admin()
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            ViewData["Title"] = "Calendar";

            return View("~/Views/website/admin/calendar.cshtml", _sampleEvents);
        }

        // ==== EMPLOYEE CALENDAR ====
        // URL: /calendar/employee
        [HttpGet("employee")]
        public IActionResult Employee()
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            ViewData["Title"] = "My Calendar";

            return View("~/Views/website/employee/calendar.cshtml", _sampleEvents);
        }
    }
}

