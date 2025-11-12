using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities; // <-- entities & enums
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace NZFTC_EMS.Controllers
{
    // ViewModels so views can stay simple
    public class LeaveRequestVm
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = "";
        public string LeaveType { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "";
        public DateTime RequestedAt { get; set; }
    }

    public class LeaveRequestFormVm
    {
        public string LeaveType { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
    }

    [Route("leave_management")]
    public class leave_management_controller : Controller
    {
        private readonly AppDbContext _context;
        public leave_management_controller(AppDbContext context) => _context = context;

        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string q, string status)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var query = _context.LeaveRequests
                .Include(l => l.Employee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(l =>
                    (l.Employee.FirstName + " " + l.Employee.LastName).Contains(q));
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                if (Enum.TryParse<LeaveStatus>(status, ignoreCase: true, out var s))
                    query = query.Where(l => l.Status == s);
            }

            var data = await query
                .OrderByDescending(l => l.RequestedAt)
                .Select(l => new LeaveRequestVm
                {
                    Id = l.LeaveRequestId,
                    EmployeeName = l.Employee.FirstName + " " + l.Employee.LastName,
                    LeaveType = l.LeaveType,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Reason = l.Reason,
                    Status = l.Status.ToString(),
                    RequestedAt = l.RequestedAt
                })
                .ToListAsync();

            ViewBag.Search = q;
            ViewBag.Status = status ?? "All";
            return View("~/Views/website/admin/leave_management.cshtml", data);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard(string status)
        {
            if (HttpContext.Session.GetString("Role") != "Employee")
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var employeeName = HttpContext.Session.GetString("Username") ?? "";
            var firstLast = employeeName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string first = firstLast.Length > 0 ? firstLast[0] : "";
            string last = firstLast.Length > 1 ? firstLast[1] : "";

            var me = await _context.Employees
                .FirstOrDefaultAsync(e => e.FirstName == first && e.LastName == last);

            var query = _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => me != null && l.EmployeeId == me.EmployeeId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                if (Enum.TryParse<LeaveStatus>(status, ignoreCase: true, out var s))
                    query = query.Where(l => l.Status == s);
            }

            var leaves = await query
                .OrderByDescending(l => l.RequestedAt)
                .Select(l => new LeaveRequestVm
                {
                    Id = l.LeaveRequestId,
                    EmployeeName = l.Employee.FirstName + " " + l.Employee.LastName,
                    LeaveType = l.LeaveType,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Reason = l.Reason,
                    Status = l.Status.ToString(),
                    RequestedAt = l.RequestedAt
                })
                .ToListAsync();

            ViewBag.Status = status ?? "All";
            return View("~/Views/website/employee/leave_form.cshtml", leaves);
        }

        [HttpGet("file")]
        public IActionResult File()
        {
            if (HttpContext.Session.GetString("Role") != "Employee")
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            return View("~/Views/website/employee/leave_request_form.cshtml",
                new LeaveRequestFormVm { StartDate = DateTime.Today, EndDate = DateTime.Today });
        }

        [HttpPost("file")]
        public async Task<IActionResult> File(LeaveRequestFormVm model)
        {
            if (HttpContext.Session.GetString("Role") != "Employee")
                return RedirectToAction("AccessDenied", "website");

            if (!ModelState.IsValid || model.EndDate < model.StartDate)
            {
                if (model.EndDate < model.StartDate)
                    ModelState.AddModelError(nameof(model.EndDate), "End Date must be after Start Date.");
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View("~/Views/website/employee/leave_request_form.cshtml", model);
            }

            // Find current employee by session name
            var employeeName = HttpContext.Session.GetString("Username") ?? "John Doe";
            var names = employeeName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var first = names.Length > 0 ? names[0] : employeeName;
            var last = names.Length > 1 ? names[1] : "";

            var emp = await _context.Employees
                .FirstOrDefaultAsync(e => e.FirstName == first && e.LastName == last);

            if (emp == null)
            {
                ModelState.AddModelError("", $"Employee record not found for '{employeeName}'. Please add an Employee first.");
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View("~/Views/website/employee/leave_request_form.cshtml", model);
            }

            var entity = new LeaveRequest
            {
                EmployeeId = emp.EmployeeId,
                LeaveType = model.LeaveType,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Reason = model.Reason,
                Status = LeaveStatus.Pending,
                RequestedAt = DateTime.UtcNow
            };

            _context.LeaveRequests.Add(entity);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Leave request submitted successfully!";
            return RedirectToAction("Dashboard");
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            if (!Enum.TryParse<LeaveStatus>(status, true, out var s))
                return BadRequest("Invalid status.");

            var req = await _context.LeaveRequests.FindAsync(id);
            if (req != null)
            {
                req.Status = s;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // Simple text export keeps your route intact
        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var leaves = await _context.LeaveRequests
                .Include(l => l.Employee).ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("Employee Name,Leave Type,Start Date,End Date,Reason,Status");
            foreach (var l in leaves)
                csv.AppendLine($"{l.Employee.FirstName} {l.Employee.LastName},{l.LeaveType},{l.StartDate:dd/MM/yyyy},{l.EndDate:dd/MM/yyyy},{l.Reason},{l.Status}");

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "Leave_Report.csv");
        }

        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            var leaves = await _context.LeaveRequests.Include(l => l.Employee).ToListAsync();
            var text = new StringBuilder();
            text.AppendLine("NZFTC Leave Management Report");
            text.AppendLine($"Generated on: {DateTime.Now}");
            text.AppendLine("=====================================\n");

            foreach (var l in leaves)
            {
                text.AppendLine($"Employee: {l.Employee.FirstName} {l.Employee.LastName}");
                text.AppendLine($"Type: {l.LeaveType}");
                text.AppendLine($"Period: {l.StartDate:dd/MM/yyyy} - {l.EndDate:dd/MM/yyyy}");
                text.AppendLine($"Reason: {l.Reason}");
                text.AppendLine($"Status: {l.Status}");
                text.AppendLine("-------------------------------------");
            }

            return File(Encoding.UTF8.GetBytes(text.ToString()), "text/plain", "Leave_Report.txt");
        }
    }
}
