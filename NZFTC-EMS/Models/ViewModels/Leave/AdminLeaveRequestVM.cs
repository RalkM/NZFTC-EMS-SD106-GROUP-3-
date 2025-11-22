using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Models.ViewModels.Leave
{
    public class AdminLeaveRequestVM
    {
        public int LeaveRequestId { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string? Department { get; set; }

        public string LeaveType { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Reason { get; set; }
        public LeaveStatus Status { get; set; }

        public DateTime RequestedAt { get; set; }

        // Admin actions
        public int? ApprovedByEmployeeId { get; set; }
        public string? AdminComment { get; set; }
        public DateTime? ApprovedAt { get; set; }

        // Balance display
        public decimal AnnualRemaining { get; set; }
        public decimal SickRemaining { get; set; }
    }
}
