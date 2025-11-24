// Data/Entities/Enums.cs
namespace NZFTC_EMS.Data.Entities
{
    public enum RateType : byte { Hourly = 0, Salary = 1 }
    public enum LeaveStatus : byte { Pending = 0, Approved = 1, Rejected = 2, Cancelled = 3 }
    public enum PayrollSummaryStatus : byte { Draft = 0, Finalized = 1, Paid = 2 }
    public enum GrievanceStatus : byte { Open = 0, InReview = 1, Resolved = 2, Closed = 3 }

    // How often an employee is paid
    public enum PayFrequency : byte
    {
        Weekly      = 0
    }

    // Status of a single payslip
    public enum PayrollStatus : byte
    {
        Processing = 0,
        Paid       = 1
    }

    // Status of a payroll run (Thu–Wed block)
    public enum PayrollRunStatus : byte
    {
        Open       = 0, // period exists but not fully calculated
        Finalizing = 1, // calculations done, waiting for confirmation
        Paid       = 2,  // run is locked and paid
        Cancelled= 3
    }

    // Timesheet approval process
    public enum TimesheetStatus : byte
    {
        Draft     = 0,
        Submitted = 1,
        Approved  = 2,
        Rejected  = 3
    }

    // Optional if you use LeaveType in LeaveRequest
    public enum LeaveType : byte
    {
        Annual = 0,
        Sick   = 1,
        Other  = 2
    }

     public enum CalendarEventType
    {
        PublicHoliday = 0,
        Meeting = 1,
        AnnualLeave = 2,
        Other = 3,
         Leave = 4
    }
}
