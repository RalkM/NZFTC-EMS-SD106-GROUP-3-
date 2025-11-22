namespace NZFTC_EMS.ViewModels.Payroll
{
    public class PayslipVM
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;

        public string PeriodCode { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public decimal PayRate { get; set; }
        public string RateType { get; set; } = string.Empty;

        public decimal GrossPay { get; set; }
        public decimal PAYE { get; set; }
        public decimal KiwiSaverEmployee { get; set; }
        public decimal KiwiSaverEmployer { get; set; }
        public decimal ACCLevy { get; set; }
        public decimal StudentLoan { get; set; }
        public decimal NetPay { get; set; }

        public DateTime GeneratedAt { get; set; }
    }
}
