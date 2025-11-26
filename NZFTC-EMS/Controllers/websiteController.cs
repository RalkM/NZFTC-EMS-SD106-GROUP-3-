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

    string role     = HttpContext.Session.GetString("Role")     ?? "Employee";
    string fullName = HttpContext.Session.GetString("FullName") ?? "Employee";

    ViewData["Layout"] = "~/Views/Shared/_portal.cshtml";

    var adminVm    = new AdminDashboardVm();
    var employeeVm = new EmployeeDashboardVm();

    bool isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                || role.Equals("HR",    StringComparison.OrdinalIgnoreCase);

    var today = DateTime.Today;

    // -----------------------------------------------------------------
    // Shared lists for "Recent Activity" + "Upcoming Dates"
    // -----------------------------------------------------------------
    var recentItems   = new List<string>();
    var upcomingItems = new List<string>();
    string summaryText = "";

    // ================================================================
    // 1) ADMIN METRICS + GLOBAL RECENT / UPCOMING
    // ================================================================
    if (isAdmin)
    {
        // EMPLOYEE + LEAVE SUMMARY
        adminVm.TotalEmployees = await _context.Employees.CountAsync();

        adminVm.ActiveLeave = await _context.LeaveRequests.CountAsync(l =>
            l.Status == LeaveStatus.Approved &&
            l.StartDate <= today &&
            l.EndDate   >= today);

        adminVm.PendingLeave = await _context.LeaveRequests
            .CountAsync(l => l.Status == LeaveStatus.Pending);

        adminVm.PendingLeaveRequests = adminVm.PendingLeave;

        // SUPPORT / GRIEVANCES
        adminVm.OpenGrievances = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.Open ||
                             t.Status == SupportStatus.InProgress);

        adminVm.PendingSupportTickets = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.Open);

        ViewBag.GrievancesInProgress = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.InProgress);

        ViewBag.GrievancesResolvedLast30Days = await _context.SupportTickets
            .CountAsync(t => t.Status == SupportStatus.Resolved &&
                             t.UpdatedAt >= DateTime.UtcNow.AddDays(-30));

        // ---------- PAYROLL OVERVIEW (real data from PayrollRuns) ----------
        var recentRuns = await _context.PayrollRuns
            .Include(r => r.Payslips)
            .OrderByDescending(r => r.PeriodEnd)
            .Take(6)
            .ToListAsync();

        if (recentRuns.Any())
        {
            var latestRun = recentRuns.First();
            var latestNetTotal = latestRun.Payslips?.Sum(p => p.NetPay) ?? 0m;

            adminVm.LatestTotalsLabel =
                $"{latestRun.PeriodStart:dd MMM}–{latestRun.PeriodEnd:dd MMM} • Net total {latestNetTotal:0.00}";

            var nextRunDate = latestRun.PeriodEnd.AddDays(7); // weekly
            adminVm.NextPayrollRunLabel = nextRunDate.ToString("dd MMM yyyy");

            ViewBag.PayrollSparkline = recentRuns
                .OrderBy(r => r.PeriodEnd)
                .Select(r => new
                {
                    Label = r.PeriodEnd.ToString("dd MMM"),
                    Value = r.Payslips?.Sum(p => p.NetPay) ?? 0m
                })
                .ToList();
        }
        else
        {
            adminVm.NextPayrollRunLabel = "soon";
            adminVm.LatestTotalsLabel   = "available in Payroll Management.";
            ViewBag.PayrollSparkline    = new List<object>();
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

        var recentPayslipsAdmin = await _context.EmployeePayrollSummaries
            .OrderByDescending(p => p.GeneratedAt)
            .Take(3)
            .ToListAsync();

        foreach (var p in recentPayslipsAdmin)
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

        // Fallback summary if no announcement rows
        summaryText =
            $"You have {adminVm.PendingLeaveRequests} pending leave request(s) " +
            $"and {adminVm.PendingSupportTickets} open support ticket(s).";
    }

    // ================================================================
    // 2) EMPLOYEE METRICS + EMPLOYEE-SPECIFIC DATA
    // ================================================================
    var emp = await _context.Employees
        .Include(e => e.LeaveBalances)
        .Include(e => e.PayrollSummaries)
            .ThenInclude(ps => ps.PayrollPeriod)
        .FirstOrDefaultAsync(e => e.EmployeeId == employeeId.Value);

    if (emp != null)
    {
        // latest leave balance
        var latestBal = emp.LeaveBalances
            .OrderByDescending(b => b.UpdatedAt)
            .FirstOrDefault();

        decimal annualRemaining = latestBal?.AnnualRemaining ?? 0m;
        decimal sickRemaining   = latestBal?.SickRemaining   ?? 0m;

        // payroll summaries
        var summaries = emp.PayrollSummaries ?? new List<EmployeePayrollSummary>();

        decimal ytd = summaries.Sum(x => x.NetPay);

        // latest payslip by GeneratedAt (safest)
        var latestSummary = summaries
            .OrderByDescending(p => p.GeneratedAt)
            .FirstOrDefault();

        decimal annualSalary = latestSummary?.PayRate ?? 0m;

        // fill VM
        employeeVm.AnnualSalary     = annualSalary;
        employeeVm.YtdEarnings      = ytd;
        employeeVm.AnnualLeaveHours = (double)annualRemaining;
        employeeVm.SickLeaveHours   = (double)sickRemaining;
        employeeVm.FullName         = $"{emp.FirstName} {emp.LastName}";
        employeeVm.Birthday         = emp.Birthday ?? DateTime.MinValue;
        employeeVm.Gender           = emp.Gender ?? "Not specified";

        // my open tickets
        ViewBag.EmployeeOpenTicketsCount = await _context.SupportTickets
            .CountAsync(t => t.EmployeeId == emp.EmployeeId &&
                             (t.Status == SupportStatus.Open ||
                              t.Status == SupportStatus.InProgress));

        // ---------- EMPLOYEE: recent payslips (last 3) ----------
        var empPayslips = summaries
            .OrderByDescending(p => p.GeneratedAt)
            .Take(3)
            .Select(p => new
            {
                p.GeneratedAt,
                p.NetPay
            })
            .ToList();

        ViewBag.EmpRecentPayslips = empPayslips;

        // ---------- EMPLOYEE: upcoming leave & history ----------
        var empUpcomingLeave = await _context.LeaveRequests
            .Where(l => l.EmployeeId == emp.EmployeeId &&
                        l.Status == LeaveStatus.Approved &&
                        l.StartDate >= today)
            .OrderBy(l => l.StartDate)
            .Take(5)
            .Select(l => new
            {
                From   = l.StartDate,
                To     = l.EndDate,
                Status = l.Status.ToString()
            })
            .ToListAsync();

        var empLeaveHistory = await _context.LeaveRequests
            .Where(l => l.EmployeeId == emp.EmployeeId &&
                        l.StartDate < today)
            .OrderByDescending(l => l.StartDate)
            .Take(5)
            .Select(l => new
            {
                From   = l.StartDate,
                To     = l.EndDate,
                Status = l.Status.ToString()
            })
            .ToListAsync();

        ViewBag.EmpUpcomingLeave = empUpcomingLeave;
        ViewBag.EmpLeaveHistory  = empLeaveHistory;

        // ---------- EMPLOYEE: latest support ticket ----------
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

        // If not admin, use a more personal dashboard summary
        if (!isAdmin)
        {
            summaryText =
                $"Annual leave remaining: {employeeVm.AnnualLeaveHours:0.##} hrs • " +
                $"Open support tickets: {ViewBag.EmployeeOpenTicketsCount ?? 0}.";
        }
    }

    // ================================================================
    // 3) Announcements (DB-backed, with summary fallback)
    // ================================================================
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

    // ensure extras are at least empty for the view
    ViewBag.EmpRecentPayslips      = ViewBag.EmpRecentPayslips      ?? new List<object>();
    ViewBag.EmpUpcomingLeave       = ViewBag.EmpUpcomingLeave       ?? new List<object>();
    ViewBag.EmpLeaveHistory        = ViewBag.EmpLeaveHistory        ?? new List<object>();
    ViewBag.EmpLatestTicketSummary = ViewBag.EmpLatestTicketSummary ?? "";
    ViewBag.EmpLatestTicketUpdated = ViewBag.EmpLatestTicketUpdated ?? "";

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
