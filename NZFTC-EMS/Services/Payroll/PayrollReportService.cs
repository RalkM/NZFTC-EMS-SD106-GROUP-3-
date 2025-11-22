using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.ViewModels.Payroll;

namespace NZFTC_EMS.Services.Payroll
{
    public class PayrollReportService
    {
        private readonly AppDbContext _db;

        public PayrollReportService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<PayrollReportVM>> GeneratePayrollReportAsync()
        {
            var summaries = await _db.EmployeePayrollSummaries
                .Include(x => x.Employee)
                .ToListAsync();

            var grouped = summaries
                .GroupBy(x => new { x.Employee.FirstName, x.Employee.LastName, x.Employee.Department })
                .Select(g => new PayrollReportVM
                {
                    EmployeeName = $"{g.Key.FirstName} {g.Key.LastName}",
                    Department = g.Key.Department ?? "N/A",

                    GrossPaid = g.Sum(x => x.GrossPay),
                    TaxPaid = g.Sum(x => x.Deductions),
                    KiwiSaverEmployeePaid = g.Sum(x => Math.Round(x.GrossPay * 0.03m, 2)),
                    ACCPaid = g.Sum(x => Math.Round(x.GrossPay * 0.0153m, 2)),

                    NetPaid = g.Sum(x => x.NetPay),
                    PayRuns = g.Count()
                })
                .ToList();

            return grouped;
        }
    }
}
