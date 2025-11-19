using System.Collections.Generic;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Models.ViewModels
{
    public class CalendarVm
    {
        public List<CalendarEvent> Events { get; set; } = new();

        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }

        public bool IsAdmin { get; set; }
    }
}

