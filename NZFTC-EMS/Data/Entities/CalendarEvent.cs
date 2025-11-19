using System;
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class CalendarEvent
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public CalendarEventType EventType { get; set; }

        // who owns this event (for employees)
        // we’ll match this with Session["Username"]
        [MaxLength(100)]
        public string? OwnerUsername { get; set; }
    }
}
