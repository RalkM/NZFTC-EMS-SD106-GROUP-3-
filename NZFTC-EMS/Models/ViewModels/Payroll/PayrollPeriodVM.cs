namespace NZFTC_EMS.Models.ViewModels.Payroll
{
    public class PayrollPeriodVM
    {
        public int PayrollPeriodId { get; set; }
        public string PeriodCode { get; set; } = string.Empty;
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        // UI
        public bool Closed { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
