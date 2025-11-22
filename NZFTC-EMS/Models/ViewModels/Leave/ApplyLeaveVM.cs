namespace NZFTC_EMS.Models.ViewModels.Leave
{
    public class ApplyLeaveVM
    {
        public int EmployeeId { get; set; }

        // Form fields
        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHalfDay { get; set; }
        public string? Reason { get; set; }

        // Calculated
        public decimal NumberOfDays { get; set; }

        // Display
        public decimal AnnualRemaining { get; set; }
        public decimal SickRemaining { get; set; }
    }
}
