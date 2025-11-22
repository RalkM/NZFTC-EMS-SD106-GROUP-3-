namespace NZFTC_EMS.ViewModels.Payroll
{
    public class PayrollSummaryVM
    {
        public int EmployeePayrollSummaryId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;

        public decimal PayRate { get; set; }
        public string RateType { get; set; } = string.Empty; // Hourly or Salary

        public decimal GrossPay { get; set; }
        public decimal PAYE { get; set; }
        public decimal KiwiSaverEmployee { get; set; }
        public decimal KiwiSaverEmployer { get; set; }
        public decimal ACCLevy { get; set; }
        public decimal StudentLoan { get; set; }
        public decimal OtherDeductions { get; set; }

        public decimal NetPay { get; set; }

        public string Status { get; set; } = string.Empty; // Draft, Finalized, Paid
    }
}
