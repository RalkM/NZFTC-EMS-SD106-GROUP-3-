using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Controllers
{
    [Route("support")]
   public class Support_ClientController : Controller
{
        private readonly AppDbContext _db;
        public Support_ClientController(AppDbContext db) => _db = db;

        // Resolve current employee id from Session or query
        private async Task<int?> GetEmployeeId(string? overrideName = null)
        {
            // Prefer EmployeeId in session
            var idStr = HttpContext.Session.GetString("EmployeeId");
            if (int.TryParse(idStr, out var eid)) return eid;

            // Fallback: try "Username" = "First Last"
            var user = overrideName ?? HttpContext.Session.GetString("Username");
            if (!string.IsNullOrWhiteSpace(user))
            {
                var parts = user.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var first = parts.ElementAtOrDefault(0) ?? "";
                var last  = string.Join(' ', parts.Skip(1));
                var emp = await _db.Employees.FirstOrDefaultAsync(e =>
                    e.FirstName == first && (string.IsNullOrEmpty(last) || e.LastName == last));
                return emp?.EmployeeId;
            }
            return null;
        }

        
    // PAGE
    [HttpGet("")]
    public IActionResult Index()
    {
        ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
        return View("~/Views/website/employee/support_form.cshtml");
    }

        // ===== API =====
        public record TicketCreateDto(string Subject, string Message, string Priority); // Low/Medium/High/Urgent
        public record TicketRowDto(int Id, string Subject, string Preview, string Status, string Priority, DateTime CreatedAt);
        public record MessageDto(int Id, int TicketId, string Body, DateTime SentAt, bool IsMine);

         [HttpGet("api/list")] public async Task<IActionResult> List()
        {
            var eid = await GetEmployeeId();
            if (eid is null) return Ok(Array.Empty<TicketRowDto>());

            var rows = await _db.SupportTickets.AsNoTracking()
                .Where(t => t.EmployeeId == eid)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TicketRowDto(
                    t.Id,
                    t.Subject,
                    t.Message.Length > 60 ? t.Message.Substring(0,60) + "…" : t.Message,
                    t.Status.ToString(),
                    t.Priority.ToString(),
                    t.CreatedAt
                ))
                .ToListAsync();

            return Ok(rows);
        }

       [HttpGet("api/{id:int}/thread")] public async Task<IActionResult> Thread(int id) 
        {
            var eid = await GetEmployeeId();
            if (eid is null) return NotFound();

            var exists = await _db.SupportTickets.AnyAsync(t => t.Id == id && t.EmployeeId == eid);
            if (!exists) return NotFound();

            var msgs = await _db.SupportMessages.AsNoTracking()
                .Where(m => m.TicketId == id)
                .OrderBy(m => m.SentAt)
                .Select(m => new MessageDto(m.Id, m.TicketId, m.Body, m.SentAt, m.SenderEmployeeId == eid))
                .ToListAsync();

            return Ok(msgs);
        }

         [ValidateAntiForgeryToken, HttpPost("api/create")] public async Task<IActionResult> Create([FromBody] TicketCreateDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Subject) || string.IsNullOrWhiteSpace(dto.Message))
                return BadRequest("Missing fields");

            var eid = await GetEmployeeId();
            if (eid is null) return BadRequest("No employee session");

            var pri = Enum.TryParse<SupportPriority>(dto.Priority ?? "Medium", true, out var p) ? p : SupportPriority.Medium;

            var t = new SupportTicket
            {
                Subject = dto.Subject.Trim(),
                Message = dto.Message.Trim(),
                Priority = pri,
                Status = SupportStatus.Open,
                EmployeeId = eid,
                CreatedAt = DateTime.UtcNow
            };
            _db.SupportTickets.Add(t);
            await _db.SaveChangesAsync();
            return Ok(new { ok = true, id = t.Id });
        }

        public record ReplyDto(string Body);

        [ValidateAntiForgeryToken, HttpPost("api/{id:int}/reply")] public async Task<IActionResult> Reply(int id, [FromBody] ReplyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Body)) return BadRequest("Empty message");

            var eid = await GetEmployeeId();
            if (eid is null) return BadRequest("No employee session");

            var t = await _db.SupportTickets.FirstOrDefaultAsync(x => x.Id == id && x.EmployeeId == eid);
            if (t is null) return NotFound();

            _db.SupportMessages.Add(new SupportMessage
            {
                TicketId = id,
                Body = dto.Body.Trim(),
                SentAt = DateTime.UtcNow,
                SenderEmployeeId = eid
            });

            t.UpdatedAt = DateTime.UtcNow;
            if (t.Status == SupportStatus.Open) t.Status = SupportStatus.InProgress;

            await _db.SaveChangesAsync();
            return Ok(new { ok = true });
        }

        [ValidateAntiForgeryToken, HttpPost("api/{id:int}/close")] public async Task<IActionResult> Close(int id)
        {
            var eid = await GetEmployeeId();
            if (eid is null) return BadRequest("No employee session");

            var t = await _db.SupportTickets.FirstOrDefaultAsync(x => x.Id == id && x.EmployeeId == eid);
            if (t is null) return NotFound();

            t.Status = SupportStatus.Closed;
            t.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { ok = true });
        }
    }
}
