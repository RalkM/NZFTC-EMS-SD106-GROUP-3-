// Data/Entities/PayGrade.cs
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class PayGrade
    {
        public int PayGradeId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        public decimal BaseRate { get; set; }  // DECIMAL(12,2)

        public RateType RateType { get; set; } // 0=Hourly,1=Salary

        public bool IsActive { get; set; } = true;
    }
}
