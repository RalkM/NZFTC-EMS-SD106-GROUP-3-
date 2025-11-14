// Data/Entities/Enums.cs
namespace NZFTC_EMS.Data.Entities
{
    public enum RateType : byte { Hourly = 0, Salary = 1 }
    public enum LeaveStatus : byte { Pending = 0, Approved = 1, Rejected = 2, Cancelled = 3 }
    public enum PayrollSummaryStatus : byte { Draft = 0, Finalized = 1, Paid = 2 }
    public enum GrievanceStatus : byte { Open = 0, InReview = 1, Resolved = 2, Closed = 3 }

}
