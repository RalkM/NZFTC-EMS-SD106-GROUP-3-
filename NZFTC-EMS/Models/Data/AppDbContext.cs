// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

    // === DbSets ===
    public DbSet<JobPosition> JobPositions => Set<JobPosition>();
    public DbSet<PayGrade> PayGrades => Set<PayGrade>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeeEmergencyContact> EmployeeEmergencyContacts => Set<EmployeeEmergencyContact>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<PayrollPeriod> PayrollPeriods => Set<PayrollPeriod>();
    public DbSet<EmployeePayrollSummary> EmployeePayrollSummaries => Set<EmployeePayrollSummary>();
    public DbSet<Grievance> Grievances => Set<Grievance>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    public DbSet<EmployeeLeaveBalance> EmployeeLeaveBalances => Set<EmployeeLeaveBalance>();

    // Support module
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<SupportMessage> SupportMessages => Set<SupportMessage>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // JobPosition
        b.Entity<JobPosition>()
            .HasIndex(x => x.Name).IsUnique();

        // PayGrade
        b.Entity<PayGrade>()
            .HasIndex(x => x.Name).IsUnique();
        b.Entity<PayGrade>()
            .Property(x => x.BaseRate).HasPrecision(12, 2);

        // Employee
        b.Entity<Employee>()
            .HasIndex(x => x.Email).IsUnique();
        b.Entity<Employee>()
            .HasIndex(x => x.EmployeeCode).IsUnique();
        b.Entity<Employee>()
            .Property(x => x.StartDate).HasColumnType("date");
       
        b.Entity<Employee>()
            .HasOne(x => x.JobPosition)
            .WithMany()
            .HasForeignKey(x => x.JobPositionId)
            .OnDelete(DeleteBehavior.Restrict);
        b.Entity<Employee>()
            .HasOne(x => x.PayGrade)
            .WithMany()
            .HasForeignKey(x => x.PayGradeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Emergency contacts
        b.Entity<EmployeeEmergencyContact>()
            .HasOne(x => x.Employee)
            .WithMany(e => e.EmergencyContacts)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // LeaveRequests
        b.Entity<LeaveRequest>()
            .Property(x => x.StartDate).HasColumnType("date");
        b.Entity<LeaveRequest>()
            .Property(x => x.EndDate).HasColumnType("date");
        b.Entity<LeaveRequest>()
            .HasOne(x => x.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
        b.Entity<LeaveRequest>()
            .HasIndex(x => new { x.StartDate, x.EndDate });

        // PayrollPeriod
        b.Entity<PayrollPeriod>()
            .HasIndex(x => x.PeriodCode).IsUnique();
        b.Entity<PayrollPeriod>()
            .Property(x => x.TotalAmount).HasPrecision(14, 2);
        b.Entity<PayrollPeriod>()
            .Property(x => x.PeriodStart).HasColumnType("date");
        b.Entity<PayrollPeriod>()
            .Property(x => x.PeriodEnd).HasColumnType("date");

        // EmployeePayrollSummary
        b.Entity<EmployeePayrollSummary>()
            .HasOne(x => x.Employee)
            .WithMany(e => e.PayrollSummaries)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
        b.Entity<EmployeePayrollSummary>()
            .HasOne(x => x.PayrollPeriod)
            .WithMany(p => p.Summaries)
            .HasForeignKey(x => x.PayrollPeriodId)
            .OnDelete(DeleteBehavior.Cascade);
        b.Entity<EmployeePayrollSummary>()
            .HasIndex(x => new { x.PayrollPeriodId, x.EmployeeId })
            .IsUnique();
        b.Entity<EmployeePayrollSummary>()
            .Property(x => x.PayRate).HasPrecision(12, 2);
        b.Entity<EmployeePayrollSummary>()
            .Property(x => x.GrossPay).HasPrecision(14, 2);
        b.Entity<EmployeePayrollSummary>()
            .Property(x => x.Deductions).HasPrecision(14, 2);

        // Use the right computed SQL for YOUR provider:
        // - MySQL (Pomelo): backticks OK (below)
        // - SQL Server: use e.HasComputedColumnSql("[GrossPay]-[Deductions]", stored: true);
        b.Entity<EmployeePayrollSummary>()
            .Property(x => x.NetPay)
            .HasPrecision(14, 2)
            .HasComputedColumnSql("(`GrossPay` - `Deductions`)", stored: true);

        // Grievance
        b.Entity<Grievance>()
            .HasOne(x => x.Employee)
            .WithMany(e => e.Grievances)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Holiday
        b.Entity<Holiday>()
            .HasIndex(x => x.HolidayDate).IsUnique();
        b.Entity<Holiday>()
            .Property(x => x.HolidayDate).HasColumnType("date");

        // ===== Support module mapping =====
        b.Entity<SupportTicket>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Subject).IsRequired().HasMaxLength(120);
            e.Property(t => t.Message).IsRequired().HasMaxLength(4000);
            e.Property(t => t.Status).HasConversion<int>();
            e.Property(t => t.Priority).HasConversion<int>();

            e.HasMany(t => t.Messages)
             .WithOne(m => m.Ticket)
             .HasForeignKey(m => m.TicketId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<SupportMessage>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Body).IsRequired().HasMaxLength(4000);
        });

        

      b.Entity<SupportTicket>().ToTable("supporttickets");
    b.Entity<SupportMessage>().ToTable("supportmessages");
        b.Entity<EmployeeLeaveBalance>(e =>
         {
             e.ToTable("employeeleavebalances");
             e.HasKey(x => x.EmployeeLeaveBalanceId);
             e.HasOne(x => x.Employee)
              .WithMany(e => e.LeaveBalances)      // add ICollection<EmployeeLeaveBalance> LeaveBalances to Employee if you want
              .HasForeignKey(x => x.EmployeeId)
              .OnDelete(DeleteBehavior.Cascade);

             e.Property(x => x.AnnualAccrued).HasPrecision(10, 2);
             e.Property(x => x.AnnualUsed).HasPrecision(10, 2);
             e.Property(x => x.SickAccrued).HasPrecision(10, 2);
             e.Property(x => x.SickUsed).HasPrecision(10, 2);
             e.Property(x => x.CarryOverAnnual).HasPrecision(10, 2);
         });

         
    }
    
}
