using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using NZFTC_EMS.ViewModels;

namespace NZFTC_EMS.Controllers
{
    public class Support_ManagementController : Controller
    {
        private readonly AppDbContext _db;
        public Support_ManagementController(AppDbContext db) => _db = db;

        [HttpGet("/support_management")]
        public async Task<IActionResult> Index(string? q, string? status, string? priority)
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var qry = _db.SupportTickets.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
                qry = qry.Where(t => EF.Functions.Like(t.Subject, $"%{q}%")
                                  || EF.Functions.Like(t.Message, $"%{q}%"));

            if (Enum.TryParse<SupportStatus>(status ?? "", true, out var st))
                qry = qry.Where(t => t.Status == st);

            if (Enum.TryParse<SupportPriority>(priority ?? "", true, out var pr))
                qry = qry.Where(t => t.Priority == pr);

            var rows = await qry
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new SupportTicketRowVm(
                    t.Id,
                    t.Subject,
                    t.Message.Length > 60 ? t.Message.Substring(0, 60) + "…" : t.Message,
                    t.Status.ToString(),
                    t.Priority.ToString(),
                    t.CreatedAt
                ))
                .ToListAsync(); // never null

            return View("~/Views/website/admin/support_management.cshtml", rows);
        }

        [HttpPost("/support_management/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupportTicketCreateVm vm)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid ticket");

            var pri = Enum.TryParse<SupportPriority>(vm.Priority, true, out var p)
                      ? p : SupportPriority.Medium;

            _db.SupportTickets.Add(new SupportTicket
            {
                Subject = vm.Subject.Trim(),
                Message = vm.Message.Trim(),
                Priority = pri,
                Status = SupportStatus.Open,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("/support_management/{id:int}/reply")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, string body)
        {
            var t = await _db.SupportTickets.FindAsync(id);
            if (t is null) return NotFound();

            _db.SupportMessages.Add(new SupportMessage
            {
                TicketId = id,
                Body = body.Trim(),
                SentAt = DateTime.UtcNow
            });

            t.UpdatedAt = DateTime.UtcNow;
            if (t.Status == SupportStatus.Open) t.Status = SupportStatus.InProgress;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("/support_management/{id:int}/status")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetStatus(int id, string status)
        {
            if (!Enum.TryParse<SupportStatus>(status, true, out var s))
                return BadRequest("Bad status");

            var t = await _db.SupportTickets.FindAsync(id);
            if (t is null) return NotFound();

            t.Status = s;
            t.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
