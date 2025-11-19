using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class PayGrade
    {
        public int PayGradeId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(255)]
        public string? Description { get; set; }   // <-- NEW

        public decimal BaseRate { get; set; }

        public RateType RateType { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
