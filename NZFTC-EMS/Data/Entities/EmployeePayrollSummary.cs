using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZFTC_EMS.Data.Entities
{
    public class EmployeePayrollSummary
    {
        public int EmployeePayrollSummaryId { get; set; }

        // Optional link to a monthly/fortnightly PayrollPeriod (for reports)
        public int? PayrollPeriodId { get; set; }
        public PayrollPeriod? PayrollPeriod { get; set; }

        // Link to Thu–Wed payroll run
        public int? PayrollRunId { get; set; }
        public PayrollRun? PayrollRun { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        // ============================
        // PAY RATE & TYPE
        // ============================
        public decimal PayRate { get; set; }          // e.g. 28.00
        public RateType RateType { get; set; }        // Hourly / Salary

        // ============================
        // FINAL PAY AMOUNTS
        // ============================
        public decimal GrossPay { get; set; }

        // DEDUCTIONS
        public decimal PAYE { get; set; }
        public decimal KiwiSaverEmployee { get; set; }
        public decimal KiwiSaverEmployer { get; set; }
        public decimal ACCLevy { get; set; }
        public decimal StudentLoan { get; set; }

        public decimal Deductions { get; set; }

        // Total hours in this run (worked + paid leave)
        public decimal TotalHours { get; set; }

        // Computed by MySQL
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal NetPay { get; set; }

        // Status + timestamps
        public PayrollSummaryStatus Status { get; set; } = PayrollSummaryStatus.Draft;
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
    }
}
