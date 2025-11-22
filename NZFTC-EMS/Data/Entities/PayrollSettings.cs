using System;

namespace NZFTC_EMS.Data.Entities
{
    public class PayrollSettings
    {
        public int PayrollSettingsId { get; set; }

        public decimal KiwiSaverEmployeePercent { get; set; }  // default 3%
        public decimal KiwiSaverEmployerPercent { get; set; }  // default 3%
        public decimal ACCLevyPercent { get; set; }            // default 1.53%

        public bool EnableStudentLoan { get; set; }           // default true

        public int RegularHoursPerWeek { get; set; }          // default 40
        public decimal OvertimeMultiplier { get; set; }       // default 1.5

        public DateTime UpdatedAt { get; set; }               // track updates
    }
}
