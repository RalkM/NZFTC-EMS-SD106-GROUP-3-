using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZFTC_EMS.Data.Entities
{
    public class EmployeePayrollSummary
    {
        public int EmployeePayrollSummaryId { get; set; }

        public int PayrollPeriodId { get; set; }
        public PayrollPeriod PayrollPeriod { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        // ============================
        // PAY RATE & TYPE
        // ============================
        public decimal PayRate { get; set; }  // DECIMAL(12,2)

        public RateType RateType { get; set; }  // Hourly / Salary


        // ============================
        // FINAL PAY AMOUNTS
        // ============================
        public decimal GrossPay { get; set; }    // DECIMAL(14,2)

        // DEDUCTIONS (all stored independently)
        public decimal PAYE { get; set; }                // DECIMAL(14,2)
        public decimal KiwiSaverEmployee { get; set; }   // DECIMAL(14,2)
        public decimal KiwiSaverEmployer { get; set; }   // DECIMAL(14,2)
        public decimal ACCLevy { get; set; }             // DECIMAL(14,2)
        public decimal StudentLoan { get; set; }         // DECIMAL(14,2)

        public decimal Deductions { get; set; }          // DECIMAL(14,2)


        // ============================
        // COMPUTED NET PAY (MySQL)
        // ============================
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal NetPay { get; private set; }


        // ============================
        // STATUS
        // ============================
        public PayrollSummaryStatus Status { get; set; } = PayrollSummaryStatus.Draft;

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}
