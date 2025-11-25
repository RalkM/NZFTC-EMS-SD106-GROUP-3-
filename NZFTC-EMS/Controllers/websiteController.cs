using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

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
        // URL: /Website/Portal
        public IActionResult Portal()
        {
            // use UserId as the "logged in" flag
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Authenticatiindex");
            ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";
            return View("~/Views/website/portal.cshtml");
        }

<<<<<<< Updated upstream
        // ===== PASSWORD HELPERS =====
=======
        // ============================================================
        // PORTAL (Admin or Employee)
        // ===========================================================

public async Task<IActionResult> Portal()
{
    int? employeeId = HttpContext.Session.GetInt32("EmployeeId");
    if (employeeId == null)
        return RedirectToAction("Authentication");

    string role     = HttpContext.Session.GetString("Role")     ?? "Employee";
    string fullName = HttpContext.Session.GetString("FullName") ?? "Employee";

    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    var adminVm    = new AdminDashboardVm();
    var employeeVm = new EmployeeDashboardVm();

    bool isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                || role.Equals("HR",    StringComparison.OrdinalIgnoreCase);

    var today = DateTime.Today;

    // -----------------------------------------------------------------
    // Prepare lists for the 3 bottom cards
    // -----------------------------------------------------------------
    var recentItems   = new List<string>(); // Recent Activity
    var upcomingItems = new List<string>(); // Upcoming Dates
    string summaryText = "";                // fallback announcement text

    // Load the logged-in employee (works for both Admin + Employee roles)
    var emp = await _context.Employees
        .Include(e => e.LeaveBalances)
        .Include(e => e.PayrollSummaries)
            .ThenInclude(ps => ps.PayrollPeriod)
        .FirstOrDefaultAsync(e => e.EmployeeId == employeeId.Value);

    // =================================================================
    // 1) ADMIN METRICS + GLOBAL RECENT / UPCOMING + PAYROLL OVERVIEW
    // =================================================================
    if (isAdmin)
    {
        // Defaults for payroll labels
        adminVm.NextPayrollRunLabel = "soon";
        adminVm.LatestTotalsLabel   = "available in Payroll Management.";

        adminVm.TotalEmployees = await _context.Employees.CountAsync();

        adminVm.ActiveLeave = await _context.LeaveRequests.CountAsync(l =>
            l.Status == LeaveStatus.Approved &&
            l.StartDate <= today &&
            l.EndDate   >= today);

        adminVm.PendingLeave = await _context.LeaveRequests
            .CountAsync(l => l.Status == LeaveStatus.Pending);

        adminVm.PendingLeaveRequests = adminVm.PendingLeave;

        adminVm.OpenGrievances = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.Open ||
                             t.Status == SupportStatus.InProgress);

        adminVm.PendingSupportTickets = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.Open);

        ViewBag.GrievancesInProgress = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.InProgress);

        ViewBag.GrievancesClosedLast30Days = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.Closed &&
                             t.UpdatedAt >= DateTime.UtcNow.AddDays(-30));

        // ---------- PAYROLL OVERVIEW ----------
        var payrollPeriods = await _context.EmployeePayrollSummaries
            .Include(s => s.PayrollPeriod)
            .Where(s => s.PayrollPeriod != null)
            .GroupBy(s => s.PayrollPeriodId)
            .Select(g => new
            {
                PeriodId    = g.Key,
                PeriodStart = g.Max(x => x.PayrollPeriod.PeriodStart),
                PeriodEnd   = g.Max(x => x.PayrollPeriod.PeriodEnd),
                TotalNet    = g.Sum(x => x.NetPay)
            })
            .OrderByDescending(x => x.PeriodEnd)
            .Take(6)
            .ToListAsync();

        if (payrollPeriods.Any())
        {
            var latest = payrollPeriods.First();

            adminVm.LatestTotalsLabel =
                $"{latest.PeriodStart:dd MMM}–{latest.PeriodEnd:dd MMM} • Net total {latest.TotalNet:0.00}";

            var nextRunDate = latest.PeriodEnd.AddDays(7); // weekly
            adminVm.NextPayrollRunLabel = nextRunDate.ToString();

            ViewBag.PayrollSparkline = payrollPeriods
                .OrderBy(x => x.PeriodEnd)
                .Select(x => new
                {
                    Label = x.PeriodEnd.ToString("dd MMM"),
                    Value = x.TotalNet
                })
                .ToList();
        }
        else
        {
            // no payroll yet
            ViewBag.PayrollSparkline = new List<object>();
        }

        // ---------- Recent Activity: GLOBAL ----------
        var recentLeaves = await _context.LeaveRequests
            .OrderByDescending(l => l.StartDate)
            .Take(3)
            .ToListAsync();

        foreach (var l in recentLeaves)
        {
            recentItems.Add(
                $"Leave • {l.Status} • {l.StartDate:dd MMM}–{l.EndDate:dd MMM}");
        }

        var recentTickets = await _context.SupportTickets
            .OrderByDescending(t => t.UpdatedAt)
            .Take(3)
            .ToListAsync();

        foreach (var t in recentTickets)
        {
            recentItems.Add($"Ticket • {t.Status} • {t.Subject}");
        }

        var recentPayslips = await _context.EmployeePayrollSummaries
            .OrderByDescending(p => p.GeneratedAt)
            .Take(3)
            .ToListAsync();

        foreach (var p in recentPayslips)
        {
            recentItems.Add($"Payroll • Payslips generated {p.GeneratedAt:dd MMM}");
        }

        // ---------- Upcoming Dates: GLOBAL (next 30 days) ----------
        var boundary = today.AddDays(30);

        var upcomingLeave = await _context.LeaveRequests
            .Where(l => l.Status == LeaveStatus.Approved &&
                        l.StartDate >= today &&
                        l.StartDate <= boundary)
            .OrderBy(l => l.StartDate)
            .Take(5)
            .ToListAsync();

        foreach (var l in upcomingLeave)
        {
            upcomingItems.Add(
                $"Leave • {l.StartDate:dd MMM}–{l.EndDate:dd MMM}");
        }

        // fallback summary for announcements if there are no rows in table
        summaryText =
            $"You have {adminVm.PendingLeaveRequests} pending leave request(s) " +
            $"and {adminVm.PendingSupportTickets} open support ticket(s).";
    }   // <--- IMPORTANT: closes if (isAdmin)

    // =================================================================
    // 2) EMPLOYEE METRICS + PERSONAL RECENT / UPCOMING
    // =================================================================
     if (emp != null)
    {
        var latestBal = emp.LeaveBalances
            .OrderByDescending(b => b.UpdatedAt)
            .FirstOrDefault();

        decimal annualRemaining = latestBal?.AnnualRemaining ?? 0m;
        decimal sickRemaining   = latestBal?.SickRemaining   ?? 0m;

        var summaries = emp.PayrollSummaries ?? new List<EmployeePayrollSummary>();
        decimal ytd   = summaries.Sum(x => x.NetPay);

        var latestSummary = summaries
            .OrderByDescending(p => p.GeneratedAt)
            .FirstOrDefault();

        decimal annualSalary = latestSummary?.PayRate ?? 0m;

        employeeVm.AnnualSalary     = annualSalary;
        employeeVm.YtdEarnings      = ytd;
        employeeVm.AnnualLeaveHours = annualRemaining;
        employeeVm.SickLeaveHours   = sickRemaining;
        employeeVm.FullName         = $"{emp.FirstName} {emp.LastName}";
        employeeVm.Birthday         = emp.Birthday ?? DateTime.MinValue;
        employeeVm.Gender           = emp.Gender ?? "Not specified";

        // my open tickets
        ViewBag.EmployeeOpenTicketsCount = await _context.SupportTickets
            .CountAsync(t => t.EmployeeId == emp.EmployeeId &&
                             (t.Status == SupportStatus.Open ||
                              t.Status == SupportStatus.InProgress));

        // latest ticket (any status)
        var latestTicket = await _context.SupportTickets
            .Where(t => t.EmployeeId == emp.EmployeeId)
            .OrderByDescending(t => t.UpdatedAt)
            .FirstOrDefaultAsync();

        if (latestTicket != null)
        {
            ViewBag.EmpLatestTicketSummary =
                $"#{latestTicket.Id} • {latestTicket.Status} • {latestTicket.Subject}";
            ViewBag.EmpLatestTicketUpdated =
                latestTicket.UpdatedAt.ToString();
        }
        else
        {
            ViewBag.EmpLatestTicketSummary = null;
            ViewBag.EmpLatestTicketUpdated = null;
        }

       var last3Payslips = summaries
    .OrderByDescending(p => p.GeneratedAt)
    .Take(3)
    .Select(p => new
    {
        GeneratedAt = p.GeneratedAt,
        NetPay      = p.NetPay,
        PeriodStart = p.PayrollPeriod != null
            ? (DateTime?)p.PayrollPeriod.PeriodStart
            : null,
        PeriodEnd = p.PayrollPeriod != null
            ? (DateTime?)p.PayrollPeriod.PeriodEnd
            : null
    })
    .ToList();

// this is what portal.cshtml is using
ViewBag.EmpRecentPayslips = last3Payslips;

        // -------------------------------------------------------------
        // EMPLOYEE LEAVE for "Upcoming / History" card
        // -------------------------------------------------------------
        var myApprovedLeaves = await _context.LeaveRequests
            .Where(l => l.EmployeeId == emp.EmployeeId &&
                        l.Status == LeaveStatus.Approved)
            .OrderBy(l => l.StartDate)
            .ToListAsync();

        // Upcoming = anything not yet finished
        employeeVm.UpcomingLeave = myApprovedLeaves
            .Where(l => l.EndDate.Date >= today)
            .OrderBy(l => l.StartDate)
            .ToList();

        // History = already finished
        employeeVm.PastLeave = myApprovedLeaves
            .Where(l => l.EndDate.Date < today)
            .OrderByDescending(l => l.StartDate)
            .ToList();

        // If NOT admin, build employee-focused Recent / Upcoming (bottom cards)
        if (!isAdmin)
        {
            var myLeaves = await _context.LeaveRequests
                .Where(l => l.EmployeeId == emp.EmployeeId)
                .OrderByDescending(l => l.StartDate)
                .Take(3)
                .ToListAsync();

            foreach (var l in myLeaves)
            {
                recentItems.Add(
                    $"Your leave • {l.Status} • {l.StartDate:dd MMM}–{l.EndDate:dd MMM}");
            }

            var myTickets = await _context.SupportTickets
                .Where(t => t.EmployeeId == emp.EmployeeId)
                .OrderByDescending(t => t.UpdatedAt)
                .Take(3)
                .ToListAsync();

            // most recent ticket for "Support / Grievances" card
            var recentTicket = myTickets
                .Select(t => new { t.Id, t.Subject, t.Status, t.UpdatedAt })
                .FirstOrDefault();

            ViewBag.RecentEmployeeTicket = recentTicket;

            foreach (var t in myTickets)
            {
                recentItems.Add($"Your ticket • {t.Status} • {t.Subject}");
            }

            var myUpcoming = myLeaves
                .Where(l => l.Status == LeaveStatus.Approved &&
                            l.StartDate >= today)
                .OrderBy(l => l.StartDate)
                .ToList();

            foreach (var l in myUpcoming)
            {
                upcomingItems.Add(
                    $"Your leave • {l.StartDate:dd MMM}–{l.EndDate:dd MMM}");
            }

            summaryText =
                $"Annual leave remaining: {employeeVm.AnnualLeaveHours:0.##} hrs • " +
                $"Open support tickets: {ViewBag.EmployeeOpenTicketsCount ?? 0}.";
        }
    }

    // =================================================================
    // 3) Announcements from table (fallback to summaryText)
    // =================================================================
    var announcements = await _context.Announcements
        .Where(a => a.IsActive)
        .OrderByDescending(a => a.CreatedAt)
        .Take(3)
        .ToListAsync();

    if (!announcements.Any() && !string.IsNullOrWhiteSpace(summaryText))
    {
        announcements.Add(new Announcement
        {
            Title     = "Dashboard summary",
            Body      = summaryText,
            CreatedAt = DateTime.UtcNow,
            IsActive  = true
        });
    }

    // -----------------------------------------------------------------
    // Push everything to the view
    // -----------------------------------------------------------------
    ViewBag.Role          = role;
    ViewBag.FullName      = fullName;
    ViewBag.AdminVm       = adminVm;
    ViewBag.EmployeeVm    = employeeVm;
    ViewBag.RecentItems   = recentItems;
    ViewBag.Upcoming      = upcomingItems;
    ViewBag.Announcements = announcements;

    return View("~/Views/website/portal.cshtml");
}







        // ============================================================
        // PASSWORD HASHING
        // ============================================================
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            // ✅ SUCCESS → set session
            HttpContext.Session.SetInt32("UserId", employee.EmployeeId);
            HttpContext.Session.SetString("UserEmail", employee.Email);
            HttpContext.Session.SetString("FirstName", employee.FirstName);
            HttpContext.Session.SetString("LastName", employee.LastName);
            HttpContext.Session.SetString("FullName", $"{employee.FirstName} {employee.LastName}");
            HttpContext.Session.SetString("Role",
                employee.JobPosition?.AccessRole ?? "Employee");
=======
            // SUCCESS → set unified session
            // SUCCESS → set unified session
HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId);
HttpContext.Session.SetString("UserEmail", employee.Email);
HttpContext.Session.SetString("FullName", $"{employee.FirstName} {employee.LastName}");
HttpContext.Session.SetString("Role", employee.JobPosition?.AccessRole ?? "Employee");

// 🔴 ADD THIS:
HttpContext.Session.SetString("Username", employee.EmployeeCode);
;
>>>>>>> Stashed changes

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

        // ============ LOGOUT ============
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
