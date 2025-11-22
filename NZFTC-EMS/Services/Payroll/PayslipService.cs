using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.ViewModels.Payroll;

namespace NZFTC_EMS.Services.Payroll
{
    public class PayslipService
    {
        private readonly AppDbContext _db;

        public PayslipService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PayslipVM> BuildPayslipAsync(int summaryId)
        {
            var summary = await _db.EmployeePayrollSummaries
                .Include(x => x.Employee)
                .Include(x => x.PayrollPeriod)
                .FirstAsync(x => x.EmployeePayrollSummaryId == summaryId);

            var employee = summary.Employee;
            var period = summary.PayrollPeriod;

            var vm = new PayslipVM
            {
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                EmployeeCode = employee.EmployeeCode ?? "N/A",
                Department = employee.Department ?? "N/A",

                PeriodCode = period.PeriodCode,
                Start = period.PeriodStart,
                End = period.PeriodEnd,

                PayRate = summary.PayRate,
                RateType = employee.PayGrade?.RateType.ToString() ?? "Unknown",

                GrossPay = summary.GrossPay,
                PAYE = summary.Deductions,
                KiwiSaverEmployee = Math.Round(summary.GrossPay * 0.03m, 2),
                KiwiSaverEmployer = Math.Round(summary.GrossPay * 0.03m, 2),
                ACCLevy = Math.Round(summary.GrossPay * 0.0153m, 2),
                StudentLoan = 0, // not tracked separately yet

                NetPay = summary.NetPay,

                GeneratedAt = DateTime.UtcNow
            };

            return vm;
        }
    }
}
