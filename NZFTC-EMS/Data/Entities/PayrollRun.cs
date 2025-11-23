using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class PayrollRun
    {
        public int PayrollRunId { get; set; }

        [DataType(DataType.Date)]
        public DateTime PeriodStart { get; set; } // usually Thursday

        [DataType(DataType.Date)]
        public DateTime PeriodEnd { get; set; }   // usually Wednesday

        // For which group of employees this run applies
        public PayFrequency PayFrequency { get; set; }

        public PayrollRunStatus Status { get; set; } = PayrollRunStatus.Open;

        public DateTime CreatedAt  { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set;}
        public DateTime? PaidAt      { get; set; }

        public ICollection<EmployeePayrollSummary> Payslips { get; set; }
            = new List<EmployeePayrollSummary>();
    }
}
