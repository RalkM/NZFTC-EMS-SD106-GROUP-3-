using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class TimesheetEntry
    {
        public int TimesheetEntryId { get; set; }

        // Link to employee
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime WorkDate { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan BreakStartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan BreakEndTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan FinishTime { get; set; }

        // Calculated when submitted/approved
        public decimal TotalHours { get; set; }

        public TimesheetStatus Status { get; set; } = TimesheetStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }

        // Optional link once run has processed it
        public int? PayrollRunId { get; set; }
        public PayrollRun? PayrollRun { get; set; }

        [MaxLength(255)]
        public string? AdminNote { get; set; }

        // 🔹 Helper: calculate total hours from start/break/finish
        public void RecalculateTotalHours()
        {
            var work = FinishTime - StartTime;
            if (work < TimeSpan.Zero) work = TimeSpan.Zero;

            var breakDuration = BreakEndTime - BreakStartTime;
            if (breakDuration < TimeSpan.Zero) breakDuration = TimeSpan.Zero;

            var net = work - breakDuration;
            if (net < TimeSpan.Zero) net = TimeSpan.Zero;

            TotalHours = (decimal)net.TotalHours;
        }

        
    }
}
