namespace NZFTC_EMS.Models.ViewModels.Leave
{
    public class LeaveReportVM
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;

        public int TotalRequests { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Pending { get; set; }

        public decimal TotalDaysTaken { get; set; }
    }
}
