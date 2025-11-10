using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace NZFTC_EMS.Controllers
{
    [Route("leave_management")]
    public class leave_management_controller : Controller
    {
        private readonly AppDbContext _context;

        public leave_management_controller(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string q, string status)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var query = _context.LeaveRequests.AsQueryable();

            if (!string.IsNullOrEmpty(q))
                query = query.Where(l => l.employee_name.Contains(q));

            if (!string.IsNullOrEmpty(status) && status != "All")
                query = query.Where(l => l.status == status);

            var requests = await query.ToListAsync();

            ViewBag.Search = q;
            ViewBag.Status = status ?? "All";

            return View("~/Views/website/admin/leave_management.cshtml", requests);
        }


        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard(string status)
        {
            if (HttpContext.Session.GetString("Role") != "Employee")
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            var employeeName = HttpContext.Session.GetString("Username");
            var query = _context.LeaveRequests.AsQueryable();

            if (!string.IsNullOrEmpty(employeeName))
                query = query.Where(l => l.employee_name == employeeName);

            if (!string.IsNullOrEmpty(status) && status != "All")
                query = query.Where(l => l.status == status);

            var leaves = await query.ToListAsync();
            ViewBag.Status = status ?? "All";

            return View("~/Views/website/employee/leave_form.cshtml", leaves);
        }


        [HttpGet("file")]
        public IActionResult File()
        {
            if (HttpContext.Session.GetString("Role") != "Employee")
                return RedirectToAction("AccessDenied", "website");

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            return View("~/Views/website/employee/leave_request_form.cshtml", new leave_request_model());
        }

        [HttpPost("file")]
        public async Task<IActionResult> File(leave_request_model model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
                return View("~/Views/website/employee/leave_request_form.cshtml", model);
            }

            model.status = "Pending";
            _context.LeaveRequests.Add(model);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Leave request submitted successfully!";
            return RedirectToAction("Dashboard");
        }


        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var req = await _context.LeaveRequests.FindAsync(id);
            if (req != null)
            {
                req.status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        [HttpGet("seed-test")]
        public async Task<IActionResult> SeedTestData()
        {
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            if (!_context.LeaveRequests.Any())
            {
                _context.LeaveRequests.AddRange(new[]
                {
                    new leave_request_model { employee_name = "John Doe", leave_type = "Annual", reason = "Vacation", start_date = DateTime.Now.AddDays(3), end_date = DateTime.Now.AddDays(7), status = "Pending" },
                    new leave_request_model { employee_name = "Jane Smith", leave_type = "Sick", reason = "Flu", start_date = DateTime.Now.AddDays(-2), end_date = DateTime.Now.AddDays(1), status = "Approved" },
                    new leave_request_model { employee_name = "Liam Brown", leave_type = "Annual", reason = "Family Event", start_date = DateTime.Now.AddDays(5), end_date = DateTime.Now.AddDays(9), status = "Declined" },
                });
                await _context.SaveChangesAsync();
            }

            TempData["msg"] = "✅ Seeded sample leave requests!";
            return RedirectToAction("Index");
        }


        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var leaves = await _context.LeaveRequests.ToListAsync();
            var csv = new StringBuilder();
            csv.AppendLine("Employee Name,Leave Type,Start Date,End Date,Reason,Status");

            foreach (var l in leaves)
                csv.AppendLine($"{l.employee_name},{l.leave_type},{l.start_date:dd/MM/yyyy},{l.end_date:dd/MM/yyyy},{l.reason},{l.status}");

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "Leave_Report.csv");
        }


        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            var leaves = await _context.LeaveRequests.ToListAsync();
            var text = new StringBuilder();
            text.AppendLine("NZFTC Leave Management Report");
            text.AppendLine($"Generated on: {DateTime.Now}");
            text.AppendLine("=====================================\n");

            foreach (var l in leaves)
            {
                text.AppendLine($"Employee: {l.employee_name}");
                text.AppendLine($"Type: {l.leave_type}");
                text.AppendLine($"Period: {l.start_date:dd/MM/yyyy} - {l.end_date:dd/MM/yyyy}");
                text.AppendLine($"Reason: {l.reason}");
                text.AppendLine($"Status: {l.status}");
                text.AppendLine("-------------------------------------");
            }

            var bytes = Encoding.UTF8.GetBytes(text.ToString());
            return File(bytes, "text/plain", "Leave_Report.txt");
        }
    }
}
