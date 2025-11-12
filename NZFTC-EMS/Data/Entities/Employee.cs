// Data/Entities/Employee.cs
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        public DateTime? Birthday { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; } = null!;

        // Store PASSWORD HASH, not plaintext.
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        public int? JobPositionId { get; set; }
        public JobPosition? JobPosition { get; set; }

        public int? PayGradeId { get; set; }
        public PayGrade? PayGrade { get; set; }

        public DateTime StartDate { get; set; }

        // Navs
        public ICollection<EmployeeEmergencyContact> EmergencyContacts { get; set; } = new List<EmployeeEmergencyContact>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<EmployeePayrollSummary> PayrollSummaries { get; set; } = new List<EmployeePayrollSummary>();
        public ICollection<Grievance> Grievances { get; set; } = new List<Grievance>();
    }
}
