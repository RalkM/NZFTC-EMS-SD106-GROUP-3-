using System;
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Models
{
    public class leave_request_model
    {
        [Key]
        public int leave_id { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        public string employee_name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Leave Type")]
        public string leave_type { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Start Date")]
        public DateTime start_date { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime end_date { get; set; }

        [Display(Name = "Reason")]
        public string? reason { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; } = "Pending";

        // ✅ Tracking fields
        [Display(Name = "Date Created")]
        public DateTime created_at { get; set; } = DateTime.Now;

        [Display(Name = "Last Updated")]
        public DateTime? updated_at { get; set; }
    }
}
