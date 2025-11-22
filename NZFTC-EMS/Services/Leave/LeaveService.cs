using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;
using NZFTC_EMS.Models.ViewModels.Leave;
using System.Text.Json;

namespace NZFTC_EMS.Services.Leave
{
    public class LeaveService
    {
        private readonly AppDbContext _db;

        public LeaveService(AppDbContext db)
        {
            _db = db;
        }

        // ===========================================================
        // APPLY LEAVE
        // ===========================================================
        public async Task<bool> ApplyLeaveAsync(ApplyLeaveVM vm)
        {
            // Validation
            if (vm.StartDate > vm.EndDate)
                throw new Exception("Start date cannot be after end date.");

            if (await HasOverlappingLeave(vm.EmployeeId, vm.StartDate, vm.EndDate))
                throw new Exception("You already have leave booked in these dates.");

            // Load balance
            var balance = await _db.EmployeeLeaveBalances
                .FirstOrDefaultAsync(x => x.EmployeeId == vm.EmployeeId);

            if (balance == null)
                throw new Exception("Leave balance not found.");

            decimal days = vm.IsHalfDay ? 0.5m : (decimal)(vm.EndDate - vm.StartDate).TotalDays + 1;

            // Validate balance
            if (vm.LeaveType == "Annual")
            {
                if (balance.AnnualAccrued - balance.AnnualUsed < days)
                    throw new Exception("Not enough annual leave available.");
            }
            else if (vm.LeaveType == "Sick")
            {
                if (balance.SickAccrued - balance.SickUsed < days)
                    throw new Exception("Not enough sick leave available.");
            }

            // Create leave request
            var request = new LeaveRequest
            {
                EmployeeId = vm.EmployeeId,
                LeaveType = vm.LeaveType,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                Reason = vm.Reason,
                Status = LeaveStatus.Pending,
                RequestedAt = DateTime.UtcNow
            };

            _db.LeaveRequests.Add(request);
            await _db.SaveChangesAsync();

            return true;
        }

        // ===========================================================
        // APPROVE / REJECT LEAVE
        // ===========================================================
        public async Task ApproveLeaveAsync(int requestId, int adminId, string? comment)
        {
            var req = await _db.LeaveRequests
                .Include(x => x.Employee)
                .FirstAsync(x => x.LeaveRequestId == requestId);

            if (req.Status != LeaveStatus.Pending)
                throw new Exception("This leave request has already been processed.");

            // Get balance
            var balance = await _db.EmployeeLeaveBalances
                .FirstAsync(x => x.EmployeeId == req.EmployeeId);

            decimal days = (decimal)(req.EndDate - req.StartDate).TotalDays + 1;

            // Deduct from balance
            if (req.LeaveType == "Annual")
                balance.AnnualUsed += days;

            else if (req.LeaveType == "Sick")
                balance.SickUsed += days;

            req.Status = LeaveStatus.Approved;
            req.ApprovedByEmployeeId = adminId;
            req.ApprovedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task RejectLeaveAsync(int requestId, int adminId, string? comment)
        {
            var req = await _db.LeaveRequests.FirstAsync(x => x.LeaveRequestId == requestId);

            if (req.Status != LeaveStatus.Pending)
                throw new Exception("This leave request has already been processed.");

            req.Status = LeaveStatus.Rejected;
            req.ApprovedByEmployeeId = adminId;
            req.ApprovedAt = DateTime.UtcNow;
            req.Reason = (req.Reason ?? "") + $"\nAdmin Comment: {comment}";

            await _db.SaveChangesAsync();
        }

        // ===========================================================
        // CANCEL LEAVE
        // ===========================================================
        public async Task CancelLeaveAsync(int requestId, int employeeId)
        {
            var req = await _db.LeaveRequests.FirstAsync(x => x.LeaveRequestId == requestId);

            if (req.EmployeeId != employeeId)
                throw new Exception("You cannot cancel someone else's leave.");

            if (req.Status != LeaveStatus.Pending)
                throw new Exception("Only pending leaves can be cancelled.");

            req.Status = LeaveStatus.Cancelled;
            await _db.SaveChangesAsync();
        }

        // ===========================================================
        // CHECK DATE OVERLAP
        // ===========================================================
        private async Task<bool> HasOverlappingLeave(int employeeId, DateTime start, DateTime end)
        {
            return await _db.LeaveRequests.AnyAsync(x =>
                x.EmployeeId == employeeId &&
                x.Status != LeaveStatus.Rejected &&
                x.Status != LeaveStatus.Cancelled &&
                (
                    (start >= x.StartDate && start <= x.EndDate) ||
                    (end >= x.StartDate && end <= x.EndDate) ||
                    (start <= x.StartDate && end >= x.EndDate)
                ));
        }

        // ===========================================================
        // ACCRUAL ENGINE (MONTHLY, PAYROLL, MANUAL)
        // ===========================================================
        public async Task RunMonthlyAccrualAsync()
        {
            var policy = await _db.LeavePolicies.FirstAsync();

            foreach (var bal in _db.EmployeeLeaveBalances)
            {
                bal.AnnualAccrued += policy.AnnualAccrualRate;
                bal.SickAccrued += policy.SickAccrualRate;
                bal.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
        }

        public async Task RunPayrollAccrualAsync()
        {
            await RunMonthlyAccrualAsync();
        }

        public async Task RunManualAccrualAsync()
        {
            await RunMonthlyAccrualAsync();
        }
    }
}
