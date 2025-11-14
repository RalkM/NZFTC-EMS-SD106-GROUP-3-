// Data/Entities/Grievance.cs
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class Grievance
    {
        public int GrievanceId { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Subject { get; set; } = null!;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public string EmployeeMessage { get; set; } = null!;
        public string? AdminResponse { get; set; }

        public GrievanceStatus Status { get; set; } = GrievanceStatus.Open;
    }
}
