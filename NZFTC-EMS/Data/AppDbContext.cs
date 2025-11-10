using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Models;

namespace NZFTC_EMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<leave_request_model> LeaveRequests { get; set; }
        public DbSet<payroll_model> Payrolls { get; set; }
    }
}
