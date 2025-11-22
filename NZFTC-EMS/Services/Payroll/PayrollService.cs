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
    }
}
