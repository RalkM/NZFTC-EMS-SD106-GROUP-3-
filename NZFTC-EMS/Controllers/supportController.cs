using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using NZFTC_EMS.Models.ViewModels;


namespace NZFTC_EMS.Controllers
{
 public class Support_ManagementController : Controller
{
    private readonly AppDbContext _db;
    public Support_ManagementController(AppDbContext db) => _db = db;

        // ================== LIST / FILTER ==================
       [HttpGet("/support_management")]
public async Task<IActionResult> Index(string? q, string? status, string? priority)
{
    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    // base query (you can add filters later if you want)
    var rows = await _db.SupportTickets
        .AsNoTracking()
        .OrderByDescending(t => t.CreatedAt)
        .GroupJoin(
            _db.Employees.AsNoTracking(),
            t => t.EmployeeId,   // match ticket.EmployeeId
            e => e.EmployeeId,   // with employee.EmployeeId
            (t, eg) => new { Ticket = t, Emp = eg.FirstOrDefault() }
        )
        .Select(x => new SupportTicketRowVm(
            x.Ticket.Id,
            x.Ticket.Subject,
            x.Ticket.Message.Length > 60
                ? x.Ticket.Message.Substring(0, 60) + "…"
                : x.Ticket.Message,
            x.Ticket.Status.ToString(),
            x.Ticket.Priority.ToString(),
            x.Ticket.CreatedAt,
            x.Emp != null ? (x.Emp.FirstName + " " + x.Emp.LastName) : null,
            x.Emp != null ? x.Emp.EmployeeCode : null,
            x.Ticket.EmployeeId
        ))
        .ToListAsync();

   return View("~/Views/website/admin/support_management.cshtml", rows);
}


        // ================== CREATE (ADMIN) ==================
        [HttpPost("/support_management/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupportTicketCreateVm vm)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid ticket");

            var pri = Enum.TryParse<SupportPriority>(vm.Priority, true, out var p)
                      ? p : SupportPriority.Medium;

            _db.SupportTickets.Add(new SupportTicket
            {
                Subject   = vm.Subject.Trim(),
                Message   = vm.Message.Trim(),
                Priority  = pri,
                Status    = SupportStatus.Open,
                CreatedAt = DateTime.UtcNow
                // EmployeeId left null → GM-only ticket on employee side
            });

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ================== REPLY ==================
        [HttpPost("/support_management/{id:int}/reply")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, string body)
        {
            var t = await _db.SupportTickets.FindAsync(id);
            if (t is null) return NotFound();

            if (string.IsNullOrWhiteSpace(body))
                return RedirectToAction(nameof(Index));

           _db.SupportMessages.Add(new SupportMessage
            {
                TicketId        = id,
                Body            = body.Trim(),
                SenderEmployeeId = null,   // optional, just to be clear it's not the employee
                SenderIsAdmin   = true,    // ✅ mark as admin
                SentAt          = DateTime.UtcNow
            });


            t.UpdatedAt = DateTime.UtcNow;
            if (t.Status == SupportStatus.Open)
                t.Status = SupportStatus.InProgress;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ================== CHANGE STATUS ==================
        [HttpPost("/support_management/{id:int}/status")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetStatus(int id, string status)
        {
            if (!Enum.TryParse<SupportStatus>(status, true, out var s))
                return BadRequest("Bad status");

            var t = await _db.SupportTickets.FindAsync(id);
            if (t is null) return NotFound();

            t.Status    = s;
            t.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public record AdminThreadMessageDto(string Body, bool FromAdmin, DateTime SentAt);

        [HttpGet("/support_management/api/{id:int}/thread")]
        public async Task<IActionResult> Thread(int id)
        {
            var msgs = await _db.SupportMessages
                .Where(m => m.TicketId == id)
                .OrderBy(m => m.SentAt)
                .Select(m => new AdminThreadMessageDto(
                    m.Body,
                    m.SenderIsAdmin,
                    m.SentAt
                ))
                .ToListAsync();

            return Ok(msgs);
        }

    }
}
