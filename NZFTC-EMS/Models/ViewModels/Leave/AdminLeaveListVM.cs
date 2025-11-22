using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Models.ViewModels.Leave
{
    public class AdminLeaveListVM
    {
        public int LeaveRequestId { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;

        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; }

        public DateTime RequestedAt { get; set; }
    }
}
