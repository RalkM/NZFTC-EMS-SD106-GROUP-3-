// Data/Entities/EmployeeTimesheet.cs
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class EmployeeTimesheet
    {
        public int EmployeeTimesheetId { get; set; }

        public int PayrollPeriodId { get; set; }
        public PayrollPeriod PayrollPeriod { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        // e.g. 2025-11-22
        public DateTime WorkDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }   // e.g. 07:00

        [Required]
        public TimeSpan EndTime { get; set; }     // e.g. 16:30

        // how many minutes of unpaid break (e.g. 30)
        public int BreakMinutes { get; set; }

        // stored so you can query easily
        public decimal TotalHours { get; set; }
    }
}
