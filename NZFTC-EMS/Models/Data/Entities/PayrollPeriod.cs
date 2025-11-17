// Data/Entities/PayrollPeriod.cs
using System.ComponentModel.DataAnnotations;


namespace NZFTC_EMS.Data.Entities
{
    public class PayrollPeriod
    {
        public int PayrollPeriodId { get; set; }

        [Required, MaxLength(50)]
        public string PeriodCode { get; set; } = null!; // e.g. 2025-11-W2

        public DateTime PeriodStart { get; set; } // DATE
        public DateTime PeriodEnd   { get; set; } // DATE

        public decimal? TotalAmount { get; set; } // DECIMAL(14,2)

        public bool Closed { get; set; } = false;

        public ICollection<EmployeePayrollSummary> Summaries { get; set; } = new List<EmployeePayrollSummary>();
    }
}
