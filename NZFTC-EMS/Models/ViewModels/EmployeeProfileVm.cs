using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NZFTC_EMS.Models.ViewModels
{
    public class EmployeeProfileVm
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = "";

        // Personal
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; } = "";

        // Contact
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";

        // Employment
        public string Department { get; set; } = "";
        public string JobTitle { get; set; } = "";
        public DateTime? StartDate { get; set; }
        public string PayFrequency { get; set; } = "";
        public string EarningsRate { get; set; } = "";

        // Emergency
        public string EmergencyName { get; set; } = "";
        public string EmergencyRelationship { get; set; } = "";
        public string EmergencyPhone { get; set; } = "";
        public string EmergencyEmail { get; set; } = "";

        public string ProfileImageUrl { get; set; } = "";
    }

    public class EmployeeProfileEditVm
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Phone { get; set; } = "";

        [Required]
        public string Address { get; set; } = "";

        public string? EmergencyName { get; set; }
        public string? EmergencyRelationship { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? EmergencyEmail { get; set; }

        public IFormFile? ProfilePhoto { get; set; }
    }
}

