using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.Data.Entities
{
    public class LeavePolicy
    {
        [Key]
        public int LeavePolicyId { get; set; }

        // Annual leave
        public decimal AnnualDefault { get; set; }
        public decimal AnnualAccrualRate { get; set; }
        public decimal AnnualCarryOverLimit { get; set; }
        public bool AllowNegativeAnnual { get; set; }

        // Sick leave
        public decimal SickDefault { get; set; }
        public decimal SickAccrualRate { get; set; }
        public bool AllowUnpaidSick { get; set; }

        // JSON for extra leave types
        public string CustomLeaveTypesJson { get; set; } = "[]";

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
