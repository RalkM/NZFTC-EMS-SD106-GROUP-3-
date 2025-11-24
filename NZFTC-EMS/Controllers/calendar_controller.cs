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

        // ---------- helpers ----------
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";
        private string? CurrentUsername => HttpContext.Session.GetString("Username");

        // ===========================================
        // MAIN PAGE (admin + employee) /calendar[/UserCalendar]
        // ===========================================
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

if (!isAdmin)
{
    var username = CurrentUsername;
    query = query.Where(e =>
        e.OwnerUsername == null ||
        (username != null && e.OwnerUsername == username));
}

            query = query.Where(e =>
                e.Start.Month == targetMonth &&
                e.Start.Year == targetYear);

            var events = await query
                .OrderBy(e => e.Start)
                .ToListAsync();

            var holidays = await _context.Holidays
                .Where(h => h.HolidayDate.Month == targetMonth && h.HolidayDate.Year == targetYear)
                .OrderBy(h => h.HolidayDate)
                .ToListAsync();

            var vm = new CalendarVm
            {
                Events = events,
                Holidays = holidays,
                CurrentMonth = targetMonth,
                CurrentYear = targetYear,
                IsAdmin = isAdmin
            };

            if (isAdmin)
                return View("~/Views/website/admin/calendar_user.cshtml", vm);

            return View("~/Views/website/employee/calendar_user.cshtml", vm);
        }

        // ===========================================
        // ADMIN – CREATE EVENT
        // GET /calendar/AdminCreate
        // ===========================================
        [HttpGet("AdminCreate")]
        public IActionResult AdminCreate(DateTime? date)
        {
            if (!IsAdmin())
                return Unauthorized();

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var start = date ?? DateTime.Today;
            var model = new CalendarEvent
            {
                Start = start,
                End = start.AddHours(1),
                EventType = CalendarEventType.Other
            };

            return View("~/Views/website/admin/calendar_create.cshtml", model);
        }

        // POST /calendar/AdminCreate
        [HttpPost("AdminCreate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreate(CalendarEvent model)
        {
            if (!IsAdmin())
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError(nameof(model.Title), "Title is required.");
            }

            if (model.End < model.Start)
            {
                ModelState.AddModelError(nameof(model.End), "End time cannot be before start time.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View("~/Views/website/admin/calendar_create.cshtml", model);
            }

            model.OwnerUsername = null;       // global admin event
            model.IsTodo = false;
            model.IsPublicHoliday = false;

            _context.CalendarEvents.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Event created.";

            return RedirectToAction(nameof(UserCalendar),
                new { month = model.Start.Month, year = model.Start.Year });
        }

        // ===========================================
        // ADMIN – EDIT EVENT
        // GET /calendar/AdminEdit/{id}
        // ===========================================
        [HttpGet("AdminEdit/{id}")]
        public async Task<IActionResult> AdminEdit(int id)
        {
            if (!IsAdmin())
                return Unauthorized();

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var entity = await _context.CalendarEvents.FindAsync(id);
            if (entity == null)
                return NotFound();

            if (entity.IsPublicHoliday)
            {
                TempData["Error"] = "Public holidays cannot be edited.";
                return RedirectToAction(nameof(UserCalendar),
                    new { month = entity.Start.Month, year = entity.Start.Year });
            }

            return View("~/Views/website/admin/calendar_edit.cshtml", entity);
        }

        // POST /calendar/AdminEdit
        [HttpPost("AdminEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEdit(CalendarEvent model)
        {
            if (!IsAdmin())
                return Unauthorized();

            var entity = await _context.CalendarEvents.FindAsync(model.Id);
            if (entity == null)
                return NotFound();

            if (entity.IsPublicHoliday)
            {
                TempData["Error"] = "Public holidays cannot be edited.";
                return RedirectToAction(nameof(UserCalendar),
                    new { month = entity.Start.Month, year = entity.Start.Year });
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                ModelState.AddModelError(nameof(model.Title), "Title is required.");
            }

            if (model.End < model.Start)
            {
                ModelState.AddModelError(nameof(model.End), "End time cannot be before start time.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View("~/Views/website/admin/calendar_edit.cshtml", model);
            }

            entity.Title = model.Title.Trim();
            entity.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
            entity.Start = model.Start;
            entity.End = model.End;
            entity.EventType = model.EventType;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Event updated.";

            return RedirectToAction(nameof(UserCalendar),
                new { month = entity.Start.Month, year = entity.Start.Year });
        }

        // ===========================================
        // ADMIN – DELETE EVENT
        // POST /calendar/AdminDelete
        // ===========================================
        [HttpPost("AdminDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDelete(int id, int month, int year)
        {
            if (!IsAdmin())
                return Unauthorized();

            var entity = await _context.CalendarEvents.FindAsync(id);
            if (entity == null)
                return NotFound();

            if (entity.IsPublicHoliday)
            {
                TempData["Error"] = "Public holidays cannot be deleted.";
                return RedirectToAction(nameof(UserCalendar), new { month, year });
            }

            _context.CalendarEvents.Remove(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Event deleted.";
            return RedirectToAction(nameof(UserCalendar), new { month, year });
        }

        // ===========================================
        // EMPLOYEE – CREATE TO-DO
        // GET /calendar/EmployeeCreateTodo
        // ===========================================
        [HttpGet("EmployeeCreateTodo")]
        public IActionResult EmployeeCreateTodo(DateTime? date)
        {
            if (IsAdmin())
                return Forbid();

            var username = CurrentUsername;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var start = date ?? DateTime.Today;
            var model = new CalendarEvent
            {
                Start = start,
                End = start.AddHours(1),
                EventType = CalendarEventType.Other
            };

            return View("~/Views/website/employee/calendar_todo_create.cshtml", model);
        }

        // POST /calendar/EmployeeCreateTodo
        [HttpPost("EmployeeCreateTodo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeCreateTodo(CalendarEvent model)
        {
            if (IsAdmin())
                return Forbid();

            var username = CurrentUsername;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(model.Title))
                ModelState.AddModelError(nameof(model.Title), "Title is required.");

            if (model.End < model.Start)
                ModelState.AddModelError(nameof(model.End), "End time cannot be before start time.");

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View("~/Views/website/employee/calendar_todo_create.cshtml", model);
            }

            model.OwnerUsername = username;
            model.IsTodo = true;
            model.IsPublicHoliday = false;
            model.EventType = CalendarEventType.Other;

            _context.CalendarEvents.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "To-do created.";

            return RedirectToAction(nameof(UserCalendar),
                new { month = model.Start.Month, year = model.Start.Year });
        }

        // ===========================================
        // EMPLOYEE – EDIT TO-DO
        // GET /calendar/EmployeeEditTodo/{id}
        // ===========================================
        [HttpGet("EmployeeEditTodo/{id}")]
        public async Task<IActionResult> EmployeeEditTodo(int id)
        {
            if (IsAdmin())
                return Forbid();

            var username = CurrentUsername;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var entity = await _context.CalendarEvents.FindAsync(id);
            if (entity == null)
                return NotFound();

            if (!entity.IsTodo || entity.OwnerUsername != username)
                return Forbid();

            return View("~/Views/website/employee/calendar_todo_edit.cshtml", entity);
        }

        // POST /calendar/EmployeeEditTodo
        [HttpPost("EmployeeEditTodo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeEditTodo(CalendarEvent model)
        {
            if (IsAdmin())
                return Forbid();

            var username = CurrentUsername;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            var entity = await _context.CalendarEvents.FindAsync(model.Id);
            if (entity == null)
                return NotFound();

            if (!entity.IsTodo || entity.OwnerUsername != username)
                return Forbid();

            if (string.IsNullOrWhiteSpace(model.Title))
                ModelState.AddModelError(nameof(model.Title), "Title is required.");

            if (model.End < model.Start)
                ModelState.AddModelError(nameof(model.End), "End time cannot be before start time.");

            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View("~/Views/website/employee/calendar_todo_edit.cshtml", model);
            }

            entity.Title = model.Title.Trim();
            entity.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
            entity.Start = model.Start;
            entity.End = model.End;

            await _context.SaveChangesAsync();

            TempData["Success"] = "To-do updated.";

            return RedirectToAction(nameof(UserCalendar),
                new { month = entity.Start.Month, year = entity.Start.Year });
        }

        // ===========================================
        // EMPLOYEE – DELETE TO-DO
        // POST /calendar/EmployeeDeleteTodo
        // ===========================================
        [HttpPost("EmployeeDeleteTodo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeDeleteTodo(int id, int month, int year)
        {
            if (IsAdmin())
                return Forbid();

            var username = CurrentUsername;
            if (string.IsNullOrWhiteSpace(username))
                return Unauthorized();

            var entity = await _context.CalendarEvents.FindAsync(id);
            if (entity == null)
                return NotFound();

            if (!entity.IsTodo || entity.OwnerUsername != username)
                return Forbid();

            _context.CalendarEvents.Remove(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "To-do deleted.";

            return RedirectToAction(nameof(UserCalendar), new { month, year });
        }


        // quick DB test
        [HttpGet("TestDb")]
        public IActionResult TestDb()
        {
            var count = _context.CalendarEvents.Count();
            return Content($"CalendarEvents in DB: {count}");
        }
    }
}

