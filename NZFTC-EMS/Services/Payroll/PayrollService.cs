using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Services.Payroll
{
    public class PayrollService
    {
        private readonly AppDbContext _db;
        private readonly TaxService _tax;

        public PayrollService(AppDbContext db, TaxService tax)
        {
            _db = db;
            _tax = tax;
        }

        // --------------------------------------------------------------
        // GENERATE PAYROLL FOR A PERIOD
        // --------------------------------------------------------------
        public async Task GeneratePayrollAsync(int payrollPeriodId)
        {
            var period = await _db.PayrollPeriods
                .FirstAsync(x => x.PayrollPeriodId == payrollPeriodId);

            if (period.Closed)
                throw new Exception("Payroll period is closed.");

            var employees = await _db.Employees
                .Include(x => x.PayGrade)
                .Where(x => x.PayGradeId != null)
                .ToListAsync();

            foreach (var emp in employees)
            {
                var grade = emp.PayGrade!;
                decimal grossWeekly = CalculateGrossWeekly(emp);

                decimal paye = _tax.CalculatePAYE(grossWeekly);
                decimal acc = _tax.CalculateACC(grossWeekly);
                decimal ksEmp = _tax.CalculateKiwiSaverEmployee(grossWeekly);
                decimal ksEmployer = _tax.CalculateKiwiSaverEmployer(grossWeekly);
                decimal sl = _tax.CalculateStudentLoan(grossWeekly);

                decimal netPay = grossWeekly - paye - acc - ksEmp - sl;

                // Save summary
                var summary = await _db.EmployeePayrollSummaries
                    .FirstOrDefaultAsync(x =>
                        x.EmployeeId == emp.EmployeeId &&
                        x.PayrollPeriodId == payrollPeriodId);

                if (summary == null)
                {
                    summary = new EmployeePayrollSummary
                    {
                        EmployeeId = emp.EmployeeId,
                        PayrollPeriodId = payrollPeriodId,
                        PayRate = grade.BaseRate,
                        GrossPay = grossWeekly,
                        Deductions = paye + acc + ksEmp + sl,
                    };

                    _db.EmployeePayrollSummaries.Add(summary);
                }
                else
                {
                    summary.PayRate = grade.BaseRate;
                    summary.GrossPay = grossWeekly;
                    summary.Deductions = paye + acc + ksEmp + sl;
                }

                await _db.SaveChangesAsync();
            }
        }

        // --------------------------------------------------------------
        // CONVERT SALARY OR HOURLY → WEEKLY PAY
        // --------------------------------------------------------------
        private decimal CalculateGrossWeekly(Employee emp)
        {
            var grade = emp.PayGrade!;
            if (grade.RateType == RateType.Salary)
            {
                // Salary is annual → convert to weekly
                return Math.Round(grade.BaseRate / 52m, 2);
            }
            else
            {
                // Assume 40 hours per week
                return Math.Round(grade.BaseRate * 40m, 2);
            }
        }

        // --------------------------------------------------------------
        // FINALIZE PAYROLL (LOCK)
        // --------------------------------------------------------------
        public async Task FinalizePayrollAsync(int payrollPeriodId)
        {
            var period = await _db.PayrollPeriods
                .FirstAsync(x => x.PayrollPeriodId == payrollPeriodId);

            period.Closed = true;

            var summaries = await _db.EmployeePayrollSummaries
                .Where(x => x.PayrollPeriodId == payrollPeriodId)
                .ToListAsync();

            foreach (var s in summaries)
            {
                s.Status = PayrollSummaryStatus.Finalized;
            }

            await _db.SaveChangesAsync();
        }

        // --------------------------------------------------------------
        // MARK PAYROLL AS PAID
        // --------------------------------------------------------------
        public async Task MarkPayrollPaidAsync(int payrollPeriodId)
        {
            var summaries = await _db.EmployeePayrollSummaries
                .Where(x => x.PayrollPeriodId == payrollPeriodId)
                .ToListAsync();

            foreach (var s in summaries)
            {
                s.Status = PayrollSummaryStatus.Paid;
            }

            await _db.SaveChangesAsync();
        }

        // --------------------------------------------------------------
// RECALCULATE ONE EMPLOYEE'S SUMMARY FROM HOURS
// --------------------------------------------------------------
public async Task RecalculateEmployeeSummaryAsync(int summaryId, decimal totalHours)
{
    var summary = await _db.EmployeePayrollSummaries
        .Include(s => s.Employee)
            .ThenInclude(e => e.PayGrade)
        .FirstOrDefaultAsync(s => s.EmployeePayrollSummaryId == summaryId);

    if (summary == null)
        throw new Exception("Payroll summary not found.");

    var emp   = summary.Employee;
    var grade = emp.PayGrade ?? throw new Exception("Employee has no pay grade.");

    // Store hours
    summary.TotalHours = totalHours;

    // Calculate gross weekly pay
    decimal grossWeekly;
    if (grade.RateType == RateType.Salary)
    {
        // annual salary -> weekly
        grossWeekly = Math.Round(grade.BaseRate / 52m, 2);
    }
    else
    {
        // hourly -> hours x rate
        grossWeekly = Math.Round(grade.BaseRate * totalHours, 2);
    }

    // NZ tax + deductions (weekly) using your TaxService
    decimal paye       = _tax.CalculatePAYE(grossWeekly);
    decimal acc        = _tax.CalculateACC(grossWeekly);
    decimal ksEmp      = _tax.CalculateKiwiSaverEmployee(grossWeekly);
    decimal ksEmployer = _tax.CalculateKiwiSaverEmployer(grossWeekly);
    decimal sl         = _tax.CalculateStudentLoan(grossWeekly);

    summary.PayRate           = grade.BaseRate;
    summary.RateType          = grade.RateType;
    summary.GrossPay          = grossWeekly;
    summary.PAYE              = paye;
    summary.KiwiSaverEmployee = ksEmp;
    summary.KiwiSaverEmployer = ksEmployer;
    summary.ACCLevy           = acc;
    summary.StudentLoan       = sl;
    summary.Deductions        = paye + acc + ksEmp + sl;

    await _db.SaveChangesAsync();
}


        // --------------------------------------------------------------
        // UPDATE HOURS FOR ONE EMPLOYEE & RECALCULATE PAYSLIP
        // --------------------------------------------------------------
        public async Task UpdateHoursAndRecalculateAsync(
            int summaryId,
            TimeSpan startTime,
            TimeSpan endTime,
            int breakMinutes)
        {
            var summary = await _db.EmployeePayrollSummaries
                .Include(s => s.Employee)
                    .ThenInclude(e => e.PayGrade)
                .FirstOrDefaultAsync(s => s.EmployeePayrollSummaryId == summaryId);

            if (summary == null)
                throw new Exception("Payroll summary not found.");

            var emp   = summary.Employee;
            var grade = emp.PayGrade ?? throw new Exception("Employee has no pay grade.");

            // 1) Calculate total hours using your helper
            var hours = CalculateTotalHours(startTime, endTime, breakMinutes);

            summary.StartTime    = startTime;
            summary.EndTime      = endTime;
            summary.BreakMinutes = breakMinutes;
            summary.TotalHours   = hours;

            // 2) Gross earnings
            decimal grossWeekly;
            if (grade.RateType == RateType.Salary)
            {
                // Salary is annual → weekly
                grossWeekly = Math.Round(grade.BaseRate / 52m, 2);
            }
            else
            {
                // Hourly → hours x rate
                grossWeekly = Math.Round(grade.BaseRate * hours, 2);
            }

            // 3) NZ-style tax/deductions using TaxService
            decimal paye       = _tax.CalculatePAYE(grossWeekly);
            decimal acc        = _tax.CalculateACC(grossWeekly);
            decimal ksEmp      = _tax.CalculateKiwiSaverEmployee(grossWeekly);
            decimal ksEmployer = _tax.CalculateKiwiSaverEmployer(grossWeekly);
            decimal sl         = _tax.CalculateStudentLoan(grossWeekly);

            // 4) Save back into summary
            summary.PayRate    = grade.BaseRate;
            summary.GrossPay   = grossWeekly;
            summary.Deductions = paye + acc + ksEmp + sl;
            // NetPay is computed in DB (you already have [DatabaseGenerated])

            await _db.SaveChangesAsync();
        }



        public decimal CalculateTotalHours(TimeSpan start, TimeSpan end, int breakMinutes)
{
    var totalMinutes = (end - start).TotalMinutes - breakMinutes;
    if (totalMinutes < 0) totalMinutes = 0; // safety

    return (decimal)(totalMinutes / 60.0);
}

    }
}
