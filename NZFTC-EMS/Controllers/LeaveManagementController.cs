using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;          // <- add this
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;            // <- add this
using NZFTC_EMS.Models.ViewModels.Leave;
using NZFTC_EMS.Services.Leave;
using NZFTC_EMS.ViewModels;


namespace NZFTC_EMS.Controllers
{
    public class LeaveManagementController : Controller
    {
        private readonly AppDbContext _db;
        private readonly LeaveService _leaveService;
        private readonly LeavePolicyService _policyService;
        private readonly LeaveReportService _reportService;

        public LeaveManagementController(
            AppDbContext db,
            LeaveService leaveService,
            LeavePolicyService policyService,
            LeaveReportService reportService)
        {
            _db = db;
            _leaveService = leaveService;
            _policyService = policyService;
            _reportService = reportService;
        }

        // ============================================================
        // NEW: INDEX ROUTE FOR /leave_management
        // ============================================================
        public IActionResult Index()
        {
            return RedirectToAction("LeaveManagement");
        }

        // ============================================================
        // EMPLOYEE PAGES
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> ApplyLeave()
        {
            int employeeId = GetEmployeeId();

            var balance = await _db.EmployeeLeaveBalances
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

            var vm = new ApplyLeaveVM
            {
                EmployeeId = employeeId,
                AnnualRemaining = (balance?.AnnualAccrued ?? 0) - (balance?.AnnualUsed ?? 0),
                SickRemaining = (balance?.SickAccrued ?? 0) - (balance?.SickUsed ?? 0)
            };

            return View("~/Views/website/employee/apply_leave.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyLeave(ApplyLeaveVM vm)
        {
            try
            {
                await _leaveService.ApplyLeaveAsync(vm);
                TempData["Success"] = "Leave request submitted successfully.";
                return RedirectToAction("LeaveHistory");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ApplyLeave");
            }
        }

        public async Task<IActionResult> LeaveHistory()
        {
            int employeeId = GetEmployeeId();

            var data = await _db.LeaveRequests
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.RequestedAt)
                .Select(x => new LeaveHistoryVM
                {
                    LeaveRequestId = x.LeaveRequestId,
                    LeaveType = x.LeaveType,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status,
                    Reason = x.Reason,
                    RequestedAt = x.RequestedAt
                })
                .ToListAsync();

            return View("~/Views/website/employee/leave_history.cshtml", data);
        }

       public async Task<IActionResult> CancelLeave(int id)
{
    int employeeId = GetEmployeeId();

    try
    {
        await _leaveService.CancelLeaveAsync(id, employeeId);

        // Remove any calendar event we created for this leave
        var req = await _db.LeaveRequests
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.LeaveRequestId == id &&
                                      x.EmployeeId == employeeId);

        if (req != null)
        {
            // IMPORTANT: this must match what you use as "Username" in session.
            // Here I'm using EmployeeCode – if your login uses email instead,
            // change this to req.Employee.Email.
            var username = req.Employee.EmployeeCode;

            var events = await _db.CalendarEvents
    .Where(e =>
        e.OwnerUsername == username &&
        e.EventType == CalendarEventType.AnnualLeave &&
        e.Start == req.StartDate &&
        e.End == req.EndDate)
    .ToListAsync();

            if (events.Any())
            {
                _db.CalendarEvents.RemoveRange(events);
                await _db.SaveChangesAsync();
            }
        }

        TempData["Success"] = "Leave request cancelled.";
    }
    catch (Exception ex)
    {
        TempData["Error"] = ex.Message;
    }

    return RedirectToAction("LeaveHistory");
}


        // ============================================================
        // ADMIN PAGES
        // ============================================================

        public async Task<IActionResult> LeaveManagement()
        {
            var vm = await _db.LeaveRequests
                .Include(x => x.Employee)
                .OrderByDescending(x => x.RequestedAt)
                .Select(x => new AdminLeaveListVM
                {
                    LeaveRequestId = x.LeaveRequestId,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    LeaveType = x.LeaveType,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status,
                    RequestedAt = x.RequestedAt
                })
                .ToListAsync();

            return View("~/Views/website/admin/leave_management.cshtml", vm);
        }

        public async Task<IActionResult> LeaveRequestDetails(int id)
        {
            var req = await _db.LeaveRequests
                .Include(x => x.Employee)
                .FirstAsync(x => x.LeaveRequestId == id);

            var balance = await _db.EmployeeLeaveBalances
                .FirstAsync(x => x.EmployeeId == req.EmployeeId);

            var vm = new AdminLeaveRequestVM
            {
                LeaveRequestId = req.LeaveRequestId,
                EmployeeId = req.EmployeeId,
                EmployeeName = $"{req.Employee.FirstName} {req.Employee.LastName}",
                Department = req.Employee.Department,
                LeaveType = req.LeaveType,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                Reason = req.Reason,
                Status = req.Status,
                RequestedAt = req.RequestedAt,
                AnnualRemaining = balance.AnnualAccrued - balance.AnnualUsed,
                SickRemaining = balance.SickAccrued - balance.SickUsed
            };

            return View("~/Views/website/admin/leave_request_details.cshtml", vm);
        }

public async Task<IActionResult> ApproveLeave(int id)
{
    int adminId = GetEmployeeId();

    // 1. Approve the leave with your existing logic
    await _leaveService.ApproveLeaveAsync(id, adminId, null);

    // 2. Load the request + employee so we can build a calendar event
    var req = await _db.LeaveRequests
        .Include(x => x.Employee)
        .FirstOrDefaultAsync(x => x.LeaveRequestId == id);

    // We avoid referencing the LeaveStatus enum directly so no extra using is needed
    if (req != null && req.Status.ToString() == "Approved")
    {
        // IMPORTANT: must match what calendar uses as CurrentUsername
        // Here I'm using EmployeeCode – if your session stores something else
        // in "Username", change this to that field (e.g. req.Employee.Email).
        var username = req.Employee.EmployeeCode;

        // Avoid duplicate events if re-approved
        var alreadyExists = await _db.CalendarEvents.AnyAsync(e =>
            e.OwnerUsername == username &&
            e.Start == req.StartDate &&
            e.End == req.EndDate);

        if (!alreadyExists)
        {
            // Start of first day
            var start = req.StartDate.Date;

            // End of last day, inclusive (23:59:59)
            var end = req.EndDate.Date
                .AddDays(1)
                .AddSeconds(-1);

            var ev = new CalendarEvent
{
    // Show both leave type + employee name
    Title = $"{req.LeaveType} - {req.Employee.FirstName} {req.Employee.LastName}",
    Description = req.Reason,

    // All-day range from first day to last day
    Start = req.StartDate.Date,
    End   = req.EndDate.Date
                .AddDays(1)
                .AddSeconds(-1),

    OwnerUsername  = username,   // so ONLY this employee + admins see it
    IsTodo         = false,
    IsPublicHoliday = false,

    // IMPORTANT: use AnnualLeave so it picks up the yellow style
    EventType = CalendarEventType.AnnualLeave
};

            _db.CalendarEvents.Add(ev);
            await _db.SaveChangesAsync();
        }
    }

    TempData["Success"] = "Leave approved.";
    return RedirectToAction("LeaveManagement");
}


[HttpPost]
public async Task<IActionResult> RejectLeave(int id, string comment)
{
    int adminId = GetEmployeeId();
    await _leaveService.RejectLeaveAsync(id, adminId, comment);
    TempData["Success"] = "Leave rejected.";
    return RedirectToAction("LeaveManagement");
}

        // ============================================================
        // ADMIN — POLICIES
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> LeavePolicies()
        {
            var vm = await _policyService.GetPolicyAsync();
            return View("~/Views/website/admin/leave_policies.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> LeavePolicies(LeavePolicyVM vm)
        {
            await _policyService.UpdatePolicyAsync(vm);
            TempData["Success"] = "Leave policies updated successfully.";
            return RedirectToAction("LeavePolicies");
        }

        // ============================================================
        // ADMIN — REPORTS
        // ============================================================

        public async Task<IActionResult> LeaveReports()
        {
            var vm = await _reportService.GenerateReportAsync();
            return View("~/Views/website/admin/leave_reports.cshtml", vm);
        }

        // ============================================================
        // ADMIN — MANUAL ACCRUAL
        // ============================================================

       // ============================================================
// ADMIN — MANUAL / MONTHLY ACCRUAL
// ============================================================

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> RunAccrual()
{
    try
    {
        // This calls LeaveService.RunMonthlyAccrualAsync()
        await _leaveService.RunManualAccrualAsync();
        TempData["Success"] = "Monthly leave accrual run successfully.";
    }
    catch (Exception ex)
    {
        TempData["Error"] = ex.Message;
    }

    return RedirectToAction("LeaveManagement");
}


        // ============================================================
        // HELPER
        // ============================================================

        private int GetEmployeeId()
        {
            var id = HttpContext.Session.GetInt32("EmployeeId");
            if (id == null)
                throw new Exception("Employee not logged in.");

            return id.Value;
        }

        
    }
}
