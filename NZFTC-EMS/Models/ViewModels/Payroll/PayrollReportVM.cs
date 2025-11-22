namespace NZFTC_EMS.ViewModels.Payroll
{
    public class PayrollReportVM
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;

        public decimal GrossPaid { get; set; }
        public decimal TaxPaid { get; set; }
        public decimal KiwiSaverEmployeePaid { get; set; }
        public decimal ACCPaid { get; set; }
        public decimal NetPaid { get; set; }

        public int PayRuns { get; set; }
    }
}
