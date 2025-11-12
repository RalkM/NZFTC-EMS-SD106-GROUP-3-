// Data/Entities/JobPosition.cs
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class JobPosition
    {
        public int JobPositionId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(400)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
