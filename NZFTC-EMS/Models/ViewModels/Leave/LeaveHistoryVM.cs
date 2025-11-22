using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Models.ViewModels.Leave
{
    public class LeaveHistoryVM
    {
        public int LeaveRequestId { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; }
        public string? Reason { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}
