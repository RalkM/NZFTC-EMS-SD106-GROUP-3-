// Data/Entities/Holiday.cs
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class Holiday
    {
        public int HolidayId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        public DateTime HolidayDate { get; set; } // DATE

        [MaxLength(50)]
        public string? HolidayType { get; set; }  // Public, Company, Regional

        public bool IsPaidHoliday { get; set; } = true;
    }
}
