using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class JobPosition
    {
        public int JobPositionId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;     // Job title

        [MaxLength(80)]
        public string? Department { get; set; }       // e.g. Accounting, HR
       public int PayGradeId { get; set; }

    // Navigation
    public PayGrade PayGrade { get; set; } = null!;

    public string? AccessRole { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
}
