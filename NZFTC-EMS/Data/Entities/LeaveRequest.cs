// Data/Entities/LeaveRequest.cs
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required, MaxLength(50)]
        public string LeaveType { get; set; } = null!; // Annual, Sick, etc.

        public DateTime StartDate { get; set; } // DATE
        public DateTime EndDate { get; set; }   // DATE

        [MaxLength(500)]
        public string? Reason { get; set; }

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public int? ApprovedByEmployeeId { get; set; }
        public Employee? ApprovedByEmployee { get; set; }

        public DateTime? ApprovedAt { get; set; }
    }
}
