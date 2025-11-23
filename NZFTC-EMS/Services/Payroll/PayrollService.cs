using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Services.Payroll
{
    public interface IPayrollService
    {
        Task<PayrollRun> CreateRunAsync(DateTime periodStart, DateTime periodEnd, PayFrequency frequency);
        Task<PayrollRun> ComputeRunAsync(int payrollRunId);
        Task<bool> MarkRunAsPaidAsync(int payrollRunId);
    }

    public class PayrollService : IPayrollService
    {
        private readonly AppDbContext _db;
        private readonly ITaxService _tax;

        public PayrollService(AppDbContext db, ITaxService tax)
        {
            _db = db;
            _tax = tax;
        }

        // 1) Create a new Thu–Wed run
        public async Task<PayrollRun> CreateRunAsync(DateTime periodStart, DateTime periodEnd, PayFrequency frequency)
        {
            var run = new PayrollRun
            {
                PeriodStart = periodStart.Date,
                PeriodEnd   = periodEnd.Date,
                PayFrequency = frequency,
                Status = PayrollRunStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            _db.PayrollRuns.Add(run);
            await _db.SaveChangesAsync();

            return run;
        }

        // 2) Compute all payslips for the run
       public async Task<PayrollRun> ComputeRunAsync(int payrollRunId)
{
    var run = await _db.PayrollRuns
        .Include(r => r.Payslips)
        .FirstOrDefaultAsync(r => r.PayrollRunId == payrollRunId);

    if (run == null)
        throw new ArgumentException("Payroll run not found.", nameof(payrollRunId));

// Only block Cancelled runs; allow recompute for Open / Finalizing / Paid
if (run.Status == PayrollRunStatus.Cancelled)
    throw new InvalidOperationException("Cancelled payroll runs cannot be processed.");

run.Status = PayrollRunStatus.Open;   // ensure a clean recompute



            // If re-running, clear old payslips
            if (run.Payslips.Any())
            {
                _db.EmployeePayrollSummaries.RemoveRange(run.Payslips);
                run.Payslips.Clear();
            }

            // Employees on this pay frequency
            var employees = await _db.Employees
                .Include(e => e.PayGrade)
                .Where(e => e.PayGradeId != null &&
                            e.PayGrade != null &&
                            e.PayFrequency == run.PayFrequency)
                .ToListAsync();

            foreach (var emp in employees)
            {
                if (emp.PayGrade == null) continue;

                // Approved timesheets for this block, not yet linked to a run
                var timesheets = await _db.TimesheetEntries
                    .Where(t => t.EmployeeId == emp.EmployeeId
                                && t.WorkDate >= run.PeriodStart
                                && t.WorkDate <= run.PeriodEnd
                                && t.Status == TimesheetStatus.Approved
                                && t.PayrollRunId == null)
                    .ToListAsync();

                decimal workedHours = timesheets.Sum(t => t.TotalHours);

                // Approved leave in this block
                var leaves = await _db.LeaveRequests
                    .Where(l => l.EmployeeId == emp.EmployeeId
                                && l.Status == LeaveStatus.Approved
                                && l.StartDate <= run.PeriodEnd
                                && l.EndDate >= run.PeriodStart)
                    .ToListAsync();

                decimal annualHours = 0m;
                decimal sickHours   = 0m;

                foreach (var leave in leaves)
                {
                    var days = (decimal)(leave.EndDate - leave.StartDate).TotalDays + 1m;
                    var hours = days * 8m; // 8 hours per day

                    if (leave.LeaveType.Equals("Annual", StringComparison.OrdinalIgnoreCase))
                    {
                        annualHours += hours;
                    }
                    else if (leave.LeaveType.Equals("Sick", StringComparison.OrdinalIgnoreCase))
                    {
                        // already capped at 8h/day
                        sickHours += hours;
                    }
                }

                // Clamp to available leave balance
                var balance = await _db.EmployeeLeaveBalances
                    .FirstOrDefaultAsync(b => b.EmployeeId == emp.EmployeeId);

                if (balance != null)
                {
                    var allowedAnnual = Math.Min(annualHours, balance.AnnualRemaining);
                    var allowedSick   = Math.Min(sickHours, balance.SickRemaining);

                    balance.AnnualUsed += allowedAnnual;
                    balance.SickUsed   += allowedSick;
                    balance.UpdatedAt   = DateTime.UtcNow;

                    annualHours = allowedAnnual;
                    sickHours   = allowedSick;
                }

                // If no leave available, it simply won’t be paid (your requirement)
                var totalPaidHours = workedHours + annualHours + sickHours;

                if (totalPaidHours <= 0)
                    continue; // nothing to pay for this employee

                var rate     = emp.PayGrade.BaseRate;
                var rateType = emp.PayGrade.RateType;

                decimal grossPay;

                if (rateType == RateType.Hourly)
                {
                    grossPay = Math.Round(totalPaidHours * rate, 2);
                }
                else
                {
                    // Simple salary split by frequency
                    grossPay = run.PayFrequency switch
                    {
                        PayFrequency.Weekly      => Math.Round(rate / 52m, 2),
                        PayFrequency.Fortnightly => Math.Round(rate / 26m, 2),
                        PayFrequency.Monthly     => Math.Round(rate / 12m, 2),
                        _                        => rate
                    };
                }

                // Tax + deductions
                var tax = _tax.CalculatePayPeriodTax(grossPay, run.PayFrequency);

                var payslip = new EmployeePayrollSummary
                {
                    EmployeeId = emp.EmployeeId,
                    PayrollRunId = run.PayrollRunId,

                    PayRate = rate,
                    RateType = rateType,

                    GrossPay = grossPay,
                    PAYE = tax.PAYE,
                    KiwiSaverEmployee = tax.OtherDeductions,
                    KiwiSaverEmployer = 0m,
                    ACCLevy = 0m,
                    StudentLoan = 0m,
                    NetPay = tax.NetPay,

                    Deductions = tax.PAYE + tax.OtherDeductions,
                    TotalHours = totalPaidHours,

                    Status = PayrollSummaryStatus.Finalized,
                    GeneratedAt = DateTime.UtcNow
                };

                run.Payslips.Add(payslip);

                // Mark timesheets as used in this run
                foreach (var ts in timesheets)
                {
                    ts.PayrollRunId = run.PayrollRunId;
                    ts.ApprovedAt ??= DateTime.UtcNow;
                }
            }

            run.Status = PayrollRunStatus.Finalizing;
            run.ProcessedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return run;
        }

        // 3) Mark as paid – updates run + each payslip
        public async Task<bool> MarkRunAsPaidAsync(int payrollRunId)
        {
            var run = await _db.PayrollRuns
                .Include(r => r.Payslips)
                .FirstOrDefaultAsync(r => r.PayrollRunId == payrollRunId);

            if (run == null) return false;

            if (run.Status == PayrollRunStatus.Paid)
                return true;

            run.Status = PayrollRunStatus.Paid;
            run.PaidAt = DateTime.UtcNow;

            foreach (var slip in run.Payslips)
            {
                slip.Status = PayrollSummaryStatus.Paid;
                slip.PaidAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
