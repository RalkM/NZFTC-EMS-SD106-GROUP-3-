// Data/Entities/EmployeeEmergencyContact.cs
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class EmployeeEmergencyContact
    {
        [Key]                                // <- add this
        public int EmergencyContactId { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string FullName { get; set; } = null!;

        [MaxLength(100)]
        public string? Relationship { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }
    }
}
