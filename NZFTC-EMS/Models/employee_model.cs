using System;
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Models
{
    public class employee_model
    {
        public int id { get; set; }

        // Basic
        [Required, StringLength(150)]
        public string full_name { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string email { get; set; } = string.Empty;

        [StringLength(80)]
        public string role { get; set; } = "Employee";

        [StringLength(80)]
        public string department { get; set; } = string.Empty;

        // Payroll
        public decimal? basic_pay { get; set; }

        // Employment
        [StringLength(120)]
        public string job_title { get; set; } = string.Empty;

        [StringLength(50)]
        public string pay_frequency { get; set; } = string.Empty; // Fortnightly/Monthly
        public DateTime? start_date { get; set; }

        // Personal
        public DateTime? birthdate { get; set; }

        [StringLength(20)]
        public string gender { get; set; } = string.Empty;

        // Contact
        [StringLength(250)]
        public string address { get; set; } = string.Empty;

        [StringLength(30)]
        public string phone { get; set; } = string.Empty;

        // Emergency contact
        [StringLength(120)]
        public string emergency_contact_name { get; set; } = string.Empty;

        [StringLength(80)]
        public string emergency_contact_relationship { get; set; } = string.Empty;

        [StringLength(30)]
        public string emergency_contact_phone { get; set; } = string.Empty;

        [StringLength(150)]
        public string emergency_contact_email { get; set; } = string.Empty;

        // Photo
        [StringLength(200)]
        public string photo_path { get; set; } = string.Empty;
    }
}




