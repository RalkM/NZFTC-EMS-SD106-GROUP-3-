using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
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

        // When someone hits /Website, send them to Authentication
        public IActionResult Index()
        {
            ViewData["Layout"] = "~/Views/Shared/index.cshtml";
            return View("~/Views/website/index.cshtml");
            }

// ============ PORTAL ============
public async Task<IActionResult> Portal()
{
    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null)
        return RedirectToAction("Authentication");

    var role     = HttpContext.Session.GetString("Role")     ?? "Employee";
    var fullName = HttpContext.Session.GetString("FullName") ?? "Employee";

    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    var adminVm    = new AdminDashboardVm();
    var employeeVm = new EmployeeDashboardVm();

    // ADMIN SIDE
    if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
        role.Equals("HR",    StringComparison.OrdinalIgnoreCase))
    {
        var today = DateTime.Today;

        adminVm.TotalEmployees = await _context.Employees.CountAsync();

        adminVm.ActiveLeave = await _context.LeaveRequests
            .CountAsync(l => l.Status == LeaveStatus.Approved &&
                             l.StartDate <= today &&
                             l.EndDate   >= today);

        adminVm.PendingLeave = await _context.LeaveRequests
            .CountAsync(l => l.Status == LeaveStatus.Pending);

        // treat support tickets as grievances
        adminVm.OpenGrievances = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.Open ||
                             t.Status == SupportStatus.InProgress);

        adminVm.PendingLeaveRequests = adminVm.PendingLeave;

        adminVm.PendingSupportTickets = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.Open);

        // extra breakdown for the donut + text
        ViewBag.GrievancesInProgress = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.InProgress);

        ViewBag.GrievancesResolvedLast30Days = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.Resolved &&
                             t.UpdatedAt >= DateTime.UtcNow.AddDays(-30));
    }
    // EMPLOYEE SIDE
    else
    {
        var emp = await _context.Employees
            .Include(e => e.LeaveBalances)
            .Include(e => e.PayrollSummaries)
                .ThenInclude(ps => ps.PayrollPeriod)
            .FirstOrDefaultAsync(e => e.EmployeeId == userId.Value);

        if (emp != null)
        {
            var latestBalance = emp.LeaveBalances
                .OrderByDescending(b => b.UpdatedAt)
                .FirstOrDefault();

            decimal annualRemaining = latestBalance?.AnnualRemaining ?? 0m;
            decimal sickRemaining   = latestBalance?.SickRemaining   ?? 0m;

            var payrollSummaries = emp.PayrollSummaries;
            decimal ytdEarnings  = payrollSummaries.Sum(p => p.NetPay);

            var latestSummary = payrollSummaries
                .OrderByDescending(p => p.PayrollPeriod.PeriodStart)
                .FirstOrDefault();

            decimal annualSalary = latestSummary?.PayRate ?? 0m;

            employeeVm.AnnualSalary     = annualSalary;
            employeeVm.YtdEarnings      = ytdEarnings;
            employeeVm.AnnualLeaveHours = (double)annualRemaining;
            employeeVm.SickLeaveHours   = (double)sickRemaining;
            employeeVm.FullName         = emp.FullName;
            employeeVm.Birthday         = emp.Birthday ?? DateTime.MinValue;
            employeeVm.Gender           = emp.Gender ?? "Not specified";

            // employee open support / grievances
            var openTicketsCount = await _context.SupportTickets
                .CountAsync(t => t.EmployeeId == emp.EmployeeId &&
                                 (t.Status == SupportStatus.Open ||
                                  t.Status == SupportStatus.InProgress));

            ViewBag.EmployeeOpenTicketsCount = openTicketsCount;
        }
    }

    ViewBag.Role       = role;
    ViewBag.FullName   = fullName;
    ViewBag.AdminVm    = adminVm;
    ViewBag.EmployeeVm = employeeVm;

    return View("~/Views/website/portal.cshtml");
}



        // ===== PASSWORD HELPERS =====
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key; // random key as salt
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA256(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }

        // ============ AUTHENTICATION (GET) ============
        // URL: /Website/Authentication
        [HttpGet]
        public IActionResult Authentication()
        {
            // already logged in → go to portal
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Portal");

            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";
            return View("~/Views/Login/login.cshtml");   // your Login folder view
        }

        // ============ AUTHENTICATION (POST) ============ 
        // (If you want the form to post here instead of Login, you can use this.
        //  Right now we’ll keep the posting on Login to match your original.)
        //
        // If you prefer to use this instead of Login POST, change your form
        // asp-action to "Authentication" and uncomment this block.
        //
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authentication(string email, string password)
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

            // FIRST-TIME LOGIN: no password set yet
            if (employee.PasswordHash == null || employee.PasswordHash.Length == 0)
            {
                CreatePasswordHash(password, out var hash, out var salt);
                employee.PasswordHash = hash;
                employee.PasswordSalt = salt;
                await _context.SaveChangesAsync();

                TempData["msg"] = "Password created. Please log in again.";
                return RedirectToAction("Authentication");
            }

            // NORMAL LOGIN: verify password
            if (employee.PasswordSalt == null ||
                !VerifyPassword(password, employee.PasswordHash!, employee.PasswordSalt))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View("~/Views/Login/login.cshtml");
            }

            // ✅ SUCCESS → set session
            HttpContext.Session.SetInt32("UserId", employee.EmployeeId);
            HttpContext.Session.SetString("UserEmail", employee.Email);
            HttpContext.Session.SetString("FirstName", employee.FirstName);
            HttpContext.Session.SetString("LastName", employee.LastName);
            HttpContext.Session.SetString("FullName", $"{employee.FirstName} {employee.LastName}");
            HttpContext.Session.SetString("Role",
                employee.JobPosition?.AccessRole ?? "Employee");

            return RedirectToAction("Portal");
        }
        */

        // ============ LOGIN (GET) ============
        [HttpGet]
        public IActionResult Login()
        {
            // use auth layout only for this page
            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";
            return View("~/Views/Login/login.cshtml");
        }

        // ============ LOGIN (POST) ============
        // This is where the form currently posts (asp-action="Login")
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            ViewData["Layout"] = "~/Views/Shared/_auth.cshtml";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                // ❌ FAIL → show same page with error
                return View("~/Views/Login/login.cshtml");
            }

            var employee = await _context.Employees
                .Include(e => e.JobPosition)
                .FirstOrDefaultAsync(e => e.Email == email);

            if (employee == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                // ❌ FAIL → show same page with error
                return View("~/Views/Login/login.cshtml");
            }

            // FIRST-TIME LOGIN: no password set yet
            if (employee.PasswordHash == null || employee.PasswordHash.Length == 0)
            {
                // Save the chosen password (setup)
                CreatePasswordHash(password, out var hash, out var salt);
                employee.PasswordHash = hash;
                employee.PasswordSalt = salt;
                await _context.SaveChangesAsync();

                TempData["msg"] = "Password created. Please log in again.";
                // Clear the form and show the login page again
                return RedirectToAction("Authentication");
            }

            // NORMAL LOGIN: verify password
            if (employee.PasswordSalt == null ||
                !VerifyPassword(password, employee.PasswordHash!, employee.PasswordSalt))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                // ❌ FAIL → show same page with error
                return View("~/Views/Login/login.cshtml");
            }

// after you’ve confirmed the employee login is valid
HttpContext.Session.SetInt32("UserId", employee.EmployeeId);
HttpContext.Session.SetString("UserEmail", employee.Email);
HttpContext.Session.SetString("FirstName", employee.FirstName);
HttpContext.Session.SetString("LastName", employee.LastName);
HttpContext.Session.SetString("FullName", $"{employee.FirstName} {employee.LastName}");
HttpContext.Session.SetString("Role", employee.JobPosition?.AccessRole ?? "Employee");

// 🔴 THESE TWO ARE CRITICAL FOR SUPPORT:
HttpContext.Session.SetString("EmployeeId", employee.EmployeeId.ToString());
HttpContext.Session.SetString("Username", $"{employee.FirstName} {employee.LastName}");



            // ✅ SUCCESS → go to /Website/Portal
            return RedirectToAction("Portal");
        }

        // ============ ADMIN DASHBOARD ============
public async Task<IActionResult> AdminDashboard()
{
    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null)
        return RedirectToAction("Authentication");

    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    var today = DateTime.Today;

    var vm = new AdminDashboardVm
    {
        TotalEmployees = await _context.Employees.CountAsync(),
        ActiveLeave    = await _context.LeaveRequests
                            .CountAsync(l => l.Status == LeaveStatus.Approved &&
                                             l.StartDate <= today &&
                                             l.EndDate   >= today),
        PendingLeave   = await _context.LeaveRequests
                            .CountAsync(l => l.Status == LeaveStatus.Pending),
        OpenGrievances = await _context.Grievances
                            .CountAsync(g => g.Status == GrievanceStatus.Open),

        PendingLeaveRequests = await _context.LeaveRequests
                                   .CountAsync(l => l.Status == LeaveStatus.Pending),
        PendingSupportTickets = 0 // hook SupportTickets later
    };

    return View("~/Views/Website/portal.cshtml", vm);
}

// ============ EMPLOYEE DASHBOARD ============
public async Task<IActionResult> EmployeeDashboard()
{
    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null)
        return RedirectToAction("Authentication");

    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    var emp = await _context.Employees
        .Include(e => e.LeaveBalances)
        .Include(e => e.PayrollSummaries)
            .ThenInclude(ps => ps.PayrollPeriod)
        .FirstOrDefaultAsync(e => e.EmployeeId == userId.Value);

    if (emp == null)
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    var latestBalance = emp.LeaveBalances
        .OrderByDescending(b => b.UpdatedAt)
        .FirstOrDefault();

    decimal annualRemaining = latestBalance?.AnnualRemaining ?? 0m;
    decimal sickRemaining   = latestBalance?.SickRemaining   ?? 0m;

    var payrollSummaries = emp.PayrollSummaries;
    decimal ytdEarnings  = payrollSummaries.Sum(p => p.NetPay);

 var latestSummary = payrollSummaries
    .OrderByDescending(p => p.PayrollPeriod.PeriodStart)
    .FirstOrDefault();


    decimal annualSalary = latestSummary?.PayRate ?? 0m;

    var vm = new EmployeeDashboardVm
    {
        AnnualSalary      = annualSalary,
        YtdEarnings       = ytdEarnings,
        AnnualLeaveHours  = (double)annualRemaining,
        SickLeaveHours    = (double)sickRemaining,
        FullName          = emp.FullName,
        Birthday          = emp.Birthday ?? DateTime.MinValue,
        Gender            = emp.Gender ?? "Not specified"
    };

    return View("~/Views/Website/portal.cshtml", vm);
}

        // ============ LOGOUT ============
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
