using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZFTC_EMS.Data.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        // PUBLIC EMPLOYEE CODE: e.g. NZFTC123456
        [MaxLength(20)]
        public string? EmployeeCode { get; set; }

        // BASIC NAME
        // BASIC NAME
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // PERSONAL
        public DateTime? Birthday { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; } = null!;

       public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        // ROLE + DEPARTMENT (UI / app logic only – not stored in DB)

        [NotMapped] 
        [MaxLength(80)] 
        public string? Role { get; set; }

        [MaxLength(80)]
        public string? Department { get; set; }

 [NotMapped]                     // 🔹 not in DB
[MaxLength(120)]
public string? JobTitle { get; set; }

        // How often this employee is paid (stored in DB)
// How often this employee is paid (Weekly / Fortnightly / Monthly)
public PayFrequency PayFrequency { get; set; } = PayFrequency.Weekly;


[NotMapped]
public decimal? BasicPay => PayGrade?.BaseRate;

        public DateTime StartDate { get; set; }

       // EMERGENCY CONTACT – UI only, not DB
        [NotMapped]
        [MaxLength(120)]
        public string? EmergencyContactName { get; set; }

        [NotMapped]
        [MaxLength(80)]
        public string? EmergencyContactRelationship { get; set; }

        [NotMapped]
        [MaxLength(30)]
        public string? EmergencyContactPhone { get; set; }

        [NotMapped]
        [MaxLength(150)]
        public string? EmergencyContactEmail { get; set; }

        [Column(TypeName = "LONGBLOB")]   // or "MEDIUMBLOB" if you prefer
        public byte[]? PhotoBytes { get; set; }
        // PHOTO
        // PHOTO – no column in employees table
        [NotMapped]
        [MaxLength(200)]
        public string? PhotoPath { get; set; }

        // LOOKUPS
        public int? JobPositionId { get; set; }
        public JobPosition? JobPosition { get; set; }

        public int? PayGradeId { get; set; }
        public PayGrade? PayGrade { get; set; }

        // NAV COLLECTIONS
        public ICollection<EmployeeEmergencyContact> EmergencyContacts { get; set; }
            = new List<EmployeeEmergencyContact>();

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
            = new List<LeaveRequest>();

        public ICollection<EmployeePayrollSummary> PayrollSummaries { get; set; }
            = new List<EmployeePayrollSummary>();

        public ICollection<EmployeeLeaveBalance> LeaveBalances { get; set; }
            = new List<EmployeeLeaveBalance>();
    }
    
}
