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

        public decimal GrossEarnings { get; set; }


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
        // HOURS WORKED (PER PERIOD)
        // ============================
        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }

        // Break duration in minutes (e.g. 30 for 30-min lunch)
        public int BreakMinutes { get; set; }

        // Total hours worked in this payroll period
        public decimal TotalHours { get; set; }


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

    // Deductions:
    public decimal PayeTax { get; set; }
    public decimal OtherDeductions { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
 
    // Optional: when actually paid
    public DateTime? PaidAt { get; set; }

    }
    }

