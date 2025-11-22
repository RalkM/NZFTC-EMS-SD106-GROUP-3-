using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data;
using NZFTC_EMS.Models.ViewModels.Leave;

namespace NZFTC_EMS.Services.Leave
{
    public class LeaveReportService
    {
        private readonly AppDbContext _db;

        public LeaveReportService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<LeaveReportVM>> GenerateReportAsync()
        {
            var data = await _db.LeaveRequests
                .Include(x => x.Employee)
                .ToListAsync();

            var grouped = data
                .GroupBy(x => new { x.Employee.FirstName, x.Employee.LastName, x.LeaveType })
                .Select(g => new LeaveReportVM
                {
                    EmployeeName = $"{g.Key.FirstName} {g.Key.LastName}",
                    LeaveType = g.Key.LeaveType,
                    TotalRequests = g.Count(),
                    Approved = g.Count(x => x.Status == Data.Entities.LeaveStatus.Approved),
                    Rejected = g.Count(x => x.Status == Data.Entities.LeaveStatus.Rejected),
                    Pending = g.Count(x => x.Status == Data.Entities.LeaveStatus.Pending),
                    TotalDaysTaken = g.Where(x => x.Status == Data.Entities.LeaveStatus.Approved)
                                      .Sum(x => (decimal)(x.EndDate - x.StartDate).TotalDays + 1)
                })
                .ToList();

            return grouped;
        }
    }
}
