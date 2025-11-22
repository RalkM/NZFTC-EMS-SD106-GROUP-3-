using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using NZFTC_EMS.Data.Entities;
using NZFTC_EMS.Models.ViewModels;

namespace NZFTC_EMS.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly AppDbContext _context;

        public WebsiteController(AppDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // INDEX (Landing page)
        // ============================================================
        public IActionResult Index()
        {
            ViewData["Layout"] = "~/Views/Shared/index.cshtml";
            return View("~/Views/website/index.cshtml");
        }

        // ============================================================
        // PORTAL (Admin or Employee)
        // ============================================================
        public async Task<IActionResult> Portal()
        {
            int? employeeId = HttpContext.Session.GetInt32("EmployeeId");
            if (employeeId == null)
                return RedirectToAction("Authentication");

            string role = HttpContext.Session.GetString("Role") ?? "Employee";
            string fullName = HttpContext.Session.GetString("FullName") ?? "Employee";

            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

            var adminVm = new AdminDashboardVm();
            var employeeVm = new EmployeeDashboardVm();

            // =====================================================================
            // ADMIN VIEW
            // =====================================================================
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("HR", StringComparison.OrdinalIgnoreCase))
            {
                var today = DateTime.Today;

                adminVm.TotalEmployees = await _context.Employees.CountAsync();

                adminVm.ActiveLeave = await _context.LeaveRequests.CountAsync(l =>
                    l.Status == LeaveStatus.Approved &&
                    l.StartDate <= today &&
                    l.EndDate >= today);

                adminVm.PendingLeave = await _context.LeaveRequests
                    .CountAsync(l => l.Status == LeaveStatus.Pending);

                adminVm.OpenGrievances = await _context.SupportTickets
                    .CountAsync(t => t.Status == SupportStatus.Open ||
                                     t.Status == SupportStatus.InProgress);

                adminVm.PendingLeaveRequests = adminVm.PendingLeave;

                adminVm.PendingSupportTickets = await _context.SupportTickets
                    .CountAsync(t => t.Status == SupportStatus.Open);

                ViewBag.GrievancesInProgress = await _context.SupportTickets
                    .CountAsync(t => t.Status == SupportStatus.InProgress);

                ViewBag.GrievancesResolvedLast30Days = await _context.SupportTickets
                    .CountAsync(t => t.Status == SupportStatus.Resolved &&
                                     t.UpdatedAt >= DateTime.UtcNow.AddDays(-30));
            }
            // =====================================================================
            // EMPLOYEE VIEW
            // =====================================================================
            else
            {
                var emp = await _context.Employees
                    .Include(e => e.LeaveBalances)
                    .Include(e => e.PayrollSummaries)
                        .ThenInclude(ps => ps.PayrollPeriod)
                    .FirstOrDefaultAsync(e => e.EmployeeId == employeeId.Value);

                if (emp != null)
                {
                    var latestBal = emp.LeaveBalances
                        .OrderByDescending(b => b.UpdatedAt)
                        .FirstOrDefault();

                    decimal annualRemaining = latestBal?.AnnualRemaining ?? 0m;
                    decimal sickRemaining = latestBal?.SickRemaining ?? 0m;

                    var summaries = emp.PayrollSummaries;
                    decimal ytd = summaries.Sum(x => x.NetPay);

                    var latestSummary = summaries
                        .OrderByDescending(p => p.PayrollPeriod.PeriodStart)
                        .FirstOrDefault();

                    decimal annualSalary = latestSummary?.PayRate ?? 0m;

                    employeeVm.AnnualSalary = annualSalary;
                    employeeVm.YtdEarnings = ytd;
                    employeeVm.AnnualLeaveHours = (double)annualRemaining;
                    employeeVm.SickLeaveHours = (double)sickRemaining;
                    employeeVm.FullName = emp.FullName;
                    employeeVm.Birthday = emp.Birthday ?? DateTime.MinValue;
                    employeeVm.Gender = emp.Gender ?? "Not specified";

                    ViewBag.EmployeeOpenTicketsCount = await _context.SupportTickets
                        .CountAsync(t => t.EmployeeId == emp.EmployeeId &&
                                         (t.Status == SupportStatus.Open ||
                                          t.Status == SupportStatus.InProgress));
                }
            }

            ViewBag.Role = role;
            ViewBag.FullName = fullName;
            ViewBag.AdminVm = adminVm;
            ViewBag.EmployeeVm = employeeVm;

            return View("~/Views/website/portal.cshtml");
        }

        // ============================================================
        // PASSWORD HASHING
        // ============================================================
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA256(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }

        // ============================================================
        // AUTHENTICATION (GET)
        // ============================================================
        [HttpGet]
        public IActionResult Authentication()
        {
            if (HttpContext.Session.GetInt32("EmployeeId") != null)
                return RedirectToAction("Portal");

            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";
            return View("~/Views/Login/login.cshtml");
        }

        // ============================================================
        // LOGIN (GET)
        // ============================================================
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";
            return View("~/Views/Login/login.cshtml");
        }

        // ============================================================
        // LOGIN (POST)
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View("~/Views/Login/login.cshtml");
            }

            var employee = await _context.Employees
                .Include(e => e.JobPosition)
                .FirstOrDefaultAsync(e => e.Email == email);

            if (employee == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View("~/Views/Login/login.cshtml");
            }

            // First-time login
            if (employee.PasswordHash == null || employee.PasswordHash.Length == 0)
            {
                CreatePasswordHash(password, out var hash, out var salt);
                employee.PasswordHash = hash;
                employee.PasswordSalt = salt;
                await _context.SaveChangesAsync();

                TempData["msg"] = "Password created. Please log in again.";
                return RedirectToAction("Authentication");
            }

            // Normal login
            if (employee.PasswordSalt == null ||
                !VerifyPassword(password, employee.PasswordHash!, employee.PasswordSalt))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View("~/Views/Login/login.cshtml");
            }

            // SUCCESS → set unified session
            HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId);
            HttpContext.Session.SetString("UserEmail", employee.Email);
            HttpContext.Session.SetString("FullName", $"{employee.FirstName} {employee.LastName}");
            HttpContext.Session.SetString("Role", employee.JobPosition?.AccessRole ?? "Employee");

            return RedirectToAction("Portal");
        }

        // ============================================================
        // LOGOUT
        // ============================================================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
