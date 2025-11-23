using System;
using System.Collections.Generic;

namespace NZFTC_EMS.Data.Entities
{
    public class PayrollPeriod
    {
        public int PayrollPeriodId { get; set; }

        // e.g. "2025-M11", "2025-FN13"
        public string PeriodCode { get; set; } = string.Empty;

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        // Monthly / fortnightly total (for summaries/reports)
        public decimal TotalAmount { get; set; }

        // Whether this period is closed for further changes
        public bool Closed { get; set; }

        // Optional navigation back to employee summaries
        public ICollection<EmployeePayrollSummary> PayrollSummaries { get; set; }
            = new List<EmployeePayrollSummary>();
    }
}
