using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using System.Linq;


namespace NZFTC_EMS.Controllers
{
    [Route("support")]
    
   public class Support_ClientController : Controller
{
        private readonly AppDbContext _db;
        public Support_ClientController(AppDbContext db) => _db = db;

        // Resolve current employee id from Session or query
       // Resolve current employee id from Session or query
        private Task<int?> GetEmployeeId()
{
    var id = HttpContext.Session.GetInt32("UserId");
    return Task.FromResult<int?>(id);
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

        // For employee-side detail view
public record TicketMessageDto(string Body, bool FromAdmin, DateTime SentAt);

public class TicketDetailDto
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;     // original ticket body
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<TicketMessageDto> Messages { get; set; } = new();
}

[HttpGet("api/{id:int}/detail")]
public async Task<IActionResult> Detail(int id)
{
    var eid = await GetEmployeeId();
    if (eid is null) return BadRequest("No employee session");

    var dto = await _db.SupportTickets
        .Where(t => t.Id == id && t.EmployeeId == eid)
        .Select(t => new TicketDetailDto
        {
            Id       = t.Id,
            Subject  = t.Subject,
            Message  = t.Message,
            Status   = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            CreatedAt = t.CreatedAt,
            Messages = t.Messages
                .OrderBy(m => m.SentAt)
                .Select(m => new TicketMessageDto(
                    m.Body,
                    m.SenderIsAdmin,   // true = admin, false = employee
                    m.SentAt
                ))
                .ToList()
        })
        .FirstOrDefaultAsync();

    if (dto is null) return NotFound();
    return Ok(dto);
}


[HttpGet("api/list")]
public async Task<IActionResult> List()
{
    var eid  = await GetEmployeeId();
    if (eid is null) return Ok(Array.Empty<TicketRowDto>());

    var role = HttpContext.Session.GetString("Role") ?? "Employee";

    var mine = _db.SupportTickets.AsNoTracking()
        .Where(t => t.EmployeeId == eid);

    // GM can see admin-created tickets too
    IQueryable<SupportTicket> query = mine;

    if (string.Equals(role, "General Manager", StringComparison.OrdinalIgnoreCase))
    {
        var gmOnly = _db.SupportTickets.AsNoTracking()
            .Where(t => t.EmployeeId == null);

        query = mine.Union(gmOnly);
    }

    var rows = await query
        .OrderByDescending(t => t.CreatedAt)
        .Select(t => new TicketRowDto(
            t.Id,
            t.Subject,
            t.Message.Length > 60 ? t.Message.Substring(0, 60) + "…" : t.Message,
            t.Status.ToString(),
            t.Priority.ToString(),
            t.CreatedAt
        ))
        .ToListAsync();

    return Ok(rows);
}

       [HttpGet("api/{id:int}/thread")]
public async Task<IActionResult> Thread(int id)
{
    var eid = await GetEmployeeId();
    if (eid is null) return Unauthorized();

    var ticket = await _db.SupportTickets.AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id);

    if (ticket is null || ticket.EmployeeId != eid)
        return NotFound();

    var msgs = await _db.SupportMessages.AsNoTracking()
        .Where(m => m.TicketId == id)
        .OrderBy(m => m.SentAt)
        .Select(m => new MessageDto(
            m.Id,
            m.TicketId,
            m.Body,
            m.SentAt,
            m.SenderEmployeeId == eid
        ))
        .ToListAsync();

    return Ok(msgs);
}
[HttpPost("api/create")]
public async Task<IActionResult> Create([FromBody] TicketCreateDto dto)
{
    if (dto is null || string.IsNullOrWhiteSpace(dto.Subject) || string.IsNullOrWhiteSpace(dto.Message))
        return BadRequest("Missing fields");

    var eid = await GetEmployeeId();
    if (eid is null) return BadRequest("No employee session");

    var pri = Enum.TryParse<SupportPriority>(dto.Priority ?? "Medium", true, out var p)
        ? p : SupportPriority.Medium;

    var t = new SupportTicket
    {
        Subject   = dto.Subject.Trim(),
        Message   = dto.Message.Trim(),
        Priority  = pri,
        Status    = SupportStatus.Open,
        EmployeeId = eid,
        CreatedAt = DateTime.UtcNow
    };

    _db.SupportTickets.Add(t);
    await _db.SaveChangesAsync();

    return Ok(new { ok = true });
}

        public record ReplyDto(string Body);


[HttpPost("api/{id:int}/reply")]
public async Task<IActionResult> Reply(int id, [FromBody] ReplyDto dto)
{
    if (dto is null || string.IsNullOrWhiteSpace(dto.Body))
        return BadRequest("Message is required");

    var eid = await GetEmployeeId();
    if (eid is null) return BadRequest("No employee session");

    var t = await _db.SupportTickets.FindAsync(id);
    if (t is null || t.EmployeeId != eid) return NotFound();

    // 🚫 do NOT allow replies on closed tickets
    if (t.Status == SupportStatus.Closed)
        return BadRequest("This ticket is closed. Please create a new ticket.");

    _db.SupportMessages.Add(new SupportMessage
    {
        TicketId         = id,
        Body             = dto.Body.Trim(),
        SenderEmployeeId = eid.Value,
        SenderIsAdmin    = false,              // 👈 make sure it shows as "You"
        SentAt           = DateTime.UtcNow
    });

    // Only reopen if it was Resolved
    if (t.Status == SupportStatus.Resolved)
        t.Status = SupportStatus.InProgress;

    t.UpdatedAt = DateTime.UtcNow;
    await _db.SaveChangesAsync();

    return Ok(new { ok = true });
}


[HttpPost("api/{id:int}/close")]
public async Task<IActionResult> Close(int id)
{
    var eid = await GetEmployeeId();
    if (eid is null) return BadRequest("No employee session");

    var t = await _db.SupportTickets
        .FirstOrDefaultAsync(x => x.Id == id && x.EmployeeId == eid);
    if (t is null) return NotFound();

    t.Status = SupportStatus.Closed;
    t.UpdatedAt = DateTime.UtcNow;
    await _db.SaveChangesAsync();
    return Ok(new { ok = true });
}
    }
}

