// Data/Entities/EmployeePayrollSummary.cs
namespace NZFTC_EMS.Data.Entities
{
    public class EmployeePayrollSummary
    {
        public int EmployeePayrollSummaryId { get; set; }

        public int PayrollPeriodId { get; set; }
        public PayrollPeriod PayrollPeriod { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public decimal PayRate { get; set; }     // DECIMAL(12,2)
        public decimal GrossPay { get; set; }    // DECIMAL(14,2)
        public decimal Deductions { get; set; }  // DECIMAL(14,2)

        // Computed (stored) in MySQL: NetPay = GrossPay - Deductions
        public decimal NetPay { get; private set; }

        public PayrollSummaryStatus Status { get; set; } = PayrollSummaryStatus.Draft;
    }
}
