using System;
using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Models
{
    public class payroll_model
    {
        [Key]
        public int payroll_id { get; set; }

        [Required]
        [Display(Name = "Employee Name")]
        public string employee_name { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Base Salary")]
        public decimal base_salary { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Deductions")]
        public decimal deductions { get; set; }

        [Display(Name = "Net Salary")]
        public decimal net_salary { get; set; }

        // ✅ Tracking fields
        [Display(Name = "Date Created")]
        public DateTime created_at { get; set; } = DateTime.Now;

        [Display(Name = "Last Updated")]
        public DateTime? updated_at { get; set; }
    }
}
