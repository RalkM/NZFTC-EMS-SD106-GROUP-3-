namespace NZFTC_EMS.ViewModels.Payroll
{
    public class PayrollSettingsVM
    {
        public decimal KiwiSaverEmployeePercent { get; set; }  // default: 3%
        public decimal KiwiSaverEmployerPercent { get; set; }  // default: 3%
        public decimal ACCLevyPercent { get; set; }            // default: 1.53%

        public bool EnableStudentLoan { get; set; }            // default: true

        public decimal RegularHoursPerWeek { get; set; }       // default: 40
        public decimal OvertimeMultiplier { get; set; }        // default: 1.5
    }
}
