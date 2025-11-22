namespace NZFTC_EMS.Models.ViewModels.Leave
{
    public class LeavePolicyVM
    {
        // Annual leave
        public decimal AnnualDefault { get; set; }
        public decimal AnnualAccrualRate { get; set; }
        public decimal AnnualCarryOverLimit { get; set; }
        public bool AllowNegativeAnnual { get; set; }

        // Sick leave
        public decimal SickDefault { get; set; }
        public decimal SickAccrualRate { get; set; }
        public bool AllowUnpaidSick { get; set; }

        // Additional types
        public List<string> CustomLeaveTypes { get; set; } = new();
    }
}
