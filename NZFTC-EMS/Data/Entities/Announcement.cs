using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class Announcement
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; } = "";

        [MaxLength(2000)]
        public string Body { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
