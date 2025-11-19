// Data/Entities/EmployeeLeaveBalance.cs
namespace NZFTC_EMS.Data.Entities;

public class EmployeeLeaveBalance
{
    public int EmployeeLeaveBalanceId { get; set; }
    public int EmployeeId { get; set; }

    public decimal AnnualAccrued { get; set; }   // hours or days
    public decimal AnnualUsed { get; set; }
    public decimal SickAccrued { get; set; }
    public decimal SickUsed { get; set; }

    public decimal CarryOverAnnual { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Employee Employee { get; set; } = null!;
    public decimal AnnualRemaining => AnnualAccrued + CarryOverAnnual - AnnualUsed;
    public decimal SickRemaining   => SickAccrued - SickUsed;
}
