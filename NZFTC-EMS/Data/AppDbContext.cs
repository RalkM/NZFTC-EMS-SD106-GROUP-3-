using System;
using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ==========================
        //        DB SETS
        // ==========================
        // ==========================
//        DB SETS
// ==========================
public DbSet<JobPosition> JobPositions => Set<JobPosition>();
public DbSet<PayGrade> PayGrades => Set<PayGrade>();
public DbSet<Employee> Employees => Set<Employee>();
public DbSet<EmployeeEmergencyContact> EmployeeEmergencyContacts => Set<EmployeeEmergencyContact>();
public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
public DbSet<PayrollPeriod> PayrollPeriods => Set<PayrollPeriod>();
public DbSet<EmployeePayrollSummary> EmployeePayrollSummaries => Set<EmployeePayrollSummary>();
public DbSet<Holiday> Holidays => Set<Holiday>();
public DbSet<EmployeeLeaveBalance> EmployeeLeaveBalances => Set<EmployeeLeaveBalance>();

public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();
public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
public DbSet<SupportMessage> SupportMessages => Set<SupportMessage>();

public DbSet<LeavePolicy> LeavePolicies => Set<LeavePolicy>();
public DbSet<PayrollSettings> PayrollSettings => Set<PayrollSettings>();

// 🔹 NEW
public DbSet<TimesheetEntry> TimesheetEntries => Set<TimesheetEntry>();
public DbSet<PayrollRun> PayrollRuns => Set<PayrollRun>();



        protected override void OnModelCreating(ModelBuilder b)

        
        {
            base.OnModelCreating(b);

            // 1. CONFIGURE ALL ENTITIES FIRST
            ConfigureJobPosition(b);
ConfigurePayGrade(b);

ConfigureEmployeeEmergencyContact(b);
ConfigureLeaveRequest(b);
ConfigurePayrollPeriod(b);
ConfigureHoliday(b);
ConfigureSupportTicket(b);
ConfigureSupportMessage(b);
ConfigureEmployeeLeaveBalance(b);
ConfigureCalendar(b);
ConfigureLeavePolicy(b);
ConfigurePayrollSettings(b);

// NEW
ConfigureTimesheetEntry(b);
ConfigurePayrollRun(b);




            // 2. SEED DATA
            SeedPayGrades(b);
            SeedJobPositions(b);
            SeedHolidays(b);
            SeedPayrollPeriods(b);
            SeedEmployees(b);
            SeedEmployeeContacts(b);
            SeedLeaveBalances(b);
            SeedSupportTickets(b);
            SeedSupportMessages(b);
            SeedCalendarEvents(b);
            SeedLeavePolicies(b);
            SeedPayrollSettings(b);

        }

        // ==========================================================
        //                    CONFIGURATION BLOCKS
        // ==========================================================
private void ConfigurePayrollRun(ModelBuilder b)
{
    b.Entity<PayrollRun>(e =>
    {
        e.ToTable("payrollruns");

        e.HasKey(x => x.PayrollRunId);

        e.Property(x => x.PeriodStart).HasColumnType("date");
        e.Property(x => x.PeriodEnd).HasColumnType("date");

        e.Property(x => x.PayFrequency)
            .HasConversion<int>();

        e.Property(x => x.Status)
            .HasConversion<int>();

        e.Property(x => x.CreatedAt).HasColumnType("datetime(6)");
        e.Property(x => x.ProcessedAt).HasColumnType("datetime(6)");
        e.Property(x => x.PaidAt).HasColumnType("datetime(6)");
    });
}

        private void ConfigureTimesheetEntry(ModelBuilder b)
{
    b.Entity<TimesheetEntry>(e =>
    {
        e.ToTable("timesheetentries");

        e.HasKey(x => x.TimesheetEntryId);

        e.Property(x => x.WorkDate).HasColumnType("date");
        e.Property(x => x.TotalHours).HasPrecision(10, 2);

        e.Property(x => x.Status)
            .HasConversion<int>();

        e.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.PayrollRun)
            .WithMany()
            .HasForeignKey(x => x.PayrollRunId)
            .OnDelete(DeleteBehavior.SetNull);
    });
}

        private void ConfigureEmployeePayrollSummary(ModelBuilder b)
{
    b.Entity<EmployeePayrollSummary>(e =>
    {
        e.ToTable("employeepayrollsummaries");

        e.HasKey(x => x.EmployeePayrollSummaryId);

        e.HasOne(x => x.Employee)
            .WithMany(e => e.PayrollSummaries)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.PayrollPeriod)
            .WithMany()
            .HasForeignKey(x => x.PayrollPeriodId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 NEW: link to payroll run
        e.HasOne(x => x.PayrollRun)
            .WithMany(r => r.Payslips)
            .HasForeignKey(x => x.PayrollRunId)
            .OnDelete(DeleteBehavior.Restrict);

        e.Property(x => x.PayRate).HasPrecision(12, 2);
        e.Property(x => x.GrossPay).HasPrecision(14, 2);
        e.Property(x => x.PAYE).HasPrecision(14, 2);
        e.Property(x => x.KiwiSaverEmployee).HasPrecision(14, 2);
        e.Property(x => x.KiwiSaverEmployer).HasPrecision(14, 2);
        e.Property(x => x.ACCLevy).HasPrecision(14, 2);
        e.Property(x => x.StudentLoan).HasPrecision(14, 2);
        e.Property(x => x.Deductions).HasPrecision(14, 2);
        e.Property(x => x.NetPay).HasPrecision(14, 2)
            .HasComputedColumnSql("`GrossPay` - `Deductions`");

        e.Property(x => x.Status)
            .HasDefaultValue(PayrollSummaryStatus.Draft);

        e.Property(x => x.RateType)
            .HasConversion<int>();
    });
}

        private void ConfigureJobPosition(ModelBuilder b)
        {
            b.Entity<JobPosition>(e =>
            {
                e.ToTable("jobpositions");

                e.HasKey(x => x.JobPositionId);

                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();

                e.Property(x => x.Department).HasMaxLength(80);
                e.Property(x => x.AccessRole).HasMaxLength(20);
                e.Property(x => x.Description).HasMaxLength(400);

                // Correct FK — prevents PayGradeId1 shadow property
                e.HasOne(x => x.PayGrade)
                    .WithMany(pg => pg.JobPositions)
                    .HasForeignKey(x => x.PayGradeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigurePayGrade(ModelBuilder b)
        {
            b.Entity<PayGrade>(e =>
            {
                e.ToTable("paygrades");

                e.HasKey(x => x.PayGradeId);

                e.Property(x => x.Name).HasMaxLength(50).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();

                e.Property(x => x.Description).HasMaxLength(255);
                e.Property(x => x.BaseRate).HasPrecision(12, 2);
                e.Property(x => x.RateType);
            });
        }

        private void ConfigureEmployee(ModelBuilder b)
        {
            b.Entity<Employee>(e =>
            {
                e.ToTable("employees");

                e.HasKey(x => x.EmployeeId);

                e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
                e.Property(x => x.LastName).HasMaxLength(100).IsRequired();

                e.Property(x => x.Email).HasMaxLength(255).IsRequired();
                e.HasIndex(x => x.Email).IsUnique();

                e.Property(x => x.EmployeeCode).HasMaxLength(20);
                e.HasIndex(x => x.EmployeeCode).IsUnique();

                e.Property(x => x.Department).HasMaxLength(80);
                e.Property(x => x.Birthday).HasColumnType("datetime(6)");
                e.Property(x => x.StartDate).HasColumnType("date");

                e.Property(x => x.PasswordHash).HasColumnType("longblob");
                e.Property(x => x.PasswordSalt).HasColumnType("longblob");

                e.HasOne(x => x.JobPosition)
                    .WithMany()
                    .HasForeignKey(x => x.JobPositionId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.PayGrade)
                    .WithMany()
                    .HasForeignKey(x => x.PayGradeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureEmployeeEmergencyContact(ModelBuilder b)
        {
            b.Entity<EmployeeEmergencyContact>(e =>
            {
                e.ToTable("employeeemergencycontacts");

                e.HasKey(x => x.EmergencyContactId);

                e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
                e.Property(x => x.Email).HasMaxLength(255);
                e.Property(x => x.Phone).HasMaxLength(30);

                e.HasOne(x => x.Employee)
                    .WithMany(e => e.EmergencyContacts)
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureLeaveRequest(ModelBuilder b)
        {
            b.Entity<LeaveRequest>(e =>
            {
                e.ToTable("leaverequests");

                e.HasKey(x => x.LeaveRequestId);

                // ENUM conversion → store as string
                e.Property(x => x.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .HasDefaultValue(LeaveStatus.Pending);

                e.Property(x => x.LeaveType).HasMaxLength(50).IsRequired();
                e.Property(x => x.Reason).HasMaxLength(500);
                e.Property(x => x.StartDate).HasColumnType("date");
                e.Property(x => x.EndDate).HasColumnType("date");
                e.Property(x => x.RequestedAt).HasColumnType("datetime(6)");

                e.HasIndex(x => new { x.StartDate, x.EndDate });

                e.HasOne(x => x.Employee)
                    .WithMany(e => e.LeaveRequests)
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                // ApprovedByEmployeeId → no cascade
                e.HasOne(x => x.ApprovedByEmployee)
                    .WithMany()
                    .HasForeignKey(x => x.ApprovedByEmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        private void ConfigurePayrollPeriod(ModelBuilder b)
        {
            b.Entity<PayrollPeriod>(e =>
            {
                e.ToTable("payrollperiods");

                e.HasKey(x => x.PayrollPeriodId);

                e.Property(x => x.PeriodCode)
                    .HasMaxLength(50)
                    .IsRequired();

                e.HasIndex(x => x.PeriodCode).IsUnique();

                e.Property(x => x.PeriodStart).HasColumnType("date");
                e.Property(x => x.PeriodEnd).HasColumnType("date");

                e.Property(x => x.TotalAmount).HasPrecision(14, 2);
            });
        }


       
        private void ConfigureHoliday(ModelBuilder b)
        {
            b.Entity<Holiday>(e =>
            {
                e.ToTable("holidays");
                e.HasKey(x => x.HolidayId);

                e.Property(x => x.Name).HasMaxLength(150).IsRequired();
                e.HasIndex(x => x.HolidayDate).IsUnique();
            });
        }

        private void ConfigureSupportTicket(ModelBuilder b)
        {
            b.Entity<SupportTicket>(e =>
            {
                e.ToTable("supporttickets");
                e.HasKey(x => x.Id);

                e.Property(x => x.Subject).HasMaxLength(120).IsRequired();
                e.Property(x => x.Message).HasMaxLength(4000).IsRequired();

                // Configure messages relationship without referencing the dependent navigation directly
                e.HasMany(x => x.Messages)
                    .WithOne() // avoid ambiguous member reference
                    .HasForeignKey("TicketId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureSupportMessage(ModelBuilder b)
        {
            b.Entity<SupportMessage>(e =>
            {
                e.ToTable("supportmessages");

                e.HasKey(x => x.Id);

                e.Property(x => x.Body)
                    .HasMaxLength(4000)
                    .IsRequired();

                e.Property(x => x.SentAt)
                    .IsRequired();


                e.HasOne(x => x.Ticket)
                    .WithMany(t => t.Messages)
                    .HasForeignKey(x => x.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }



        private void ConfigureEmployeeLeaveBalance(ModelBuilder b)
        {
            b.Entity<EmployeeLeaveBalance>(e =>
            {
                e.ToTable("employeeleavebalances");

                e.HasKey(x => x.EmployeeLeaveBalanceId);

                e.Property(x => x.UpdatedAt).HasColumnType("datetime(6)");

                e.HasOne(x => x.Employee)
                    .WithMany(e => e.LeaveBalances)
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        private void ConfigureCalendar(ModelBuilder b)
        {
            b.Entity<CalendarEvent>(e =>
            { e.ToTable("calendarevents"); });
        }
        private void ConfigureLeavePolicy(ModelBuilder b)
        {
            b.Entity<LeavePolicy>(e =>
            {
                e.ToTable("leavepolicies");
                e.HasKey(x => x.LeavePolicyId);

                e.Property(x => x.CustomLeaveTypesJson).HasColumnType("longtext");
            });
        }
        private void ConfigurePayrollSettings(ModelBuilder b)
        {
            b.Entity<PayrollSettings>(e =>
            {
                e.ToTable("payrollsettings");

                e.HasKey(x => x.PayrollSettingsId);

                e.Property(x => x.KiwiSaverEmployeePercent).HasPrecision(5, 2);
                e.Property(x => x.KiwiSaverEmployerPercent).HasPrecision(5, 2);
                e.Property(x => x.ACCLevyPercent).HasPrecision(5, 3);
                e.Property(x => x.OvertimeMultiplier).HasPrecision(5, 2);
                e.Property(x => x.UpdatedAt).HasColumnType("datetime(6)");
            });
        }

        private void SeedPayGrades(ModelBuilder b)
        {
            b.Entity<PayGrade>().HasData(
                new PayGrade { PayGradeId = 1, Name = "PG-minimum", Description = "Minimum Pay", BaseRate = 23.50m, RateType = RateType.Hourly, IsActive = true },
                new PayGrade { PayGradeId = 2, Name = "PG-REG", Description = "Regular Staff", BaseRate = 28.00m, RateType = RateType.Hourly, IsActive = true },
                new PayGrade { PayGradeId = 3, Name = "PG-SSTAFF", Description = "Senior Staff", BaseRate = 32.00m, RateType = RateType.Hourly, IsActive = true },
                new PayGrade { PayGradeId = 4, Name = "PG-TL", Description = "Team Leader", BaseRate = 38.00m, RateType = RateType.Hourly, IsActive = true },
                new PayGrade { PayGradeId = 5, Name = "PG-SUP", Description = "Supervisor", BaseRate = 45.00m, RateType = RateType.Hourly, IsActive = true },
                new PayGrade { PayGradeId = 6, Name = "PG-SPEC", Description = "Specialist", BaseRate = 50.00m, RateType = RateType.Hourly, IsActive = true },
                new PayGrade { PayGradeId = 7, Name = "PG-SAL-JM", Description = "Salary - Junior/Assistant Manager", BaseRate = 65000m, RateType = RateType.Salary, IsActive = true },
                new PayGrade { PayGradeId = 8, Name = "PG-SAL-MAN", Description = "Salary - Department Manager", BaseRate = 80000m, RateType = RateType.Salary, IsActive = true },
                new PayGrade { PayGradeId = 9, Name = "PG-SAL-SRMAN", Description = "Salary - Senior Manager", BaseRate = 95000m, RateType = RateType.Salary, IsActive = true },
                new PayGrade { PayGradeId = 10, Name = "PG-SAL-GM", Description = "Salary - General Manager", BaseRate = 100000m, RateType = RateType.Salary, IsActive = true }
            );
        }

        private void SeedJobPositions(ModelBuilder b)
        {
            b.Entity<JobPosition>().HasData(

                // ===== FINANCE =====
                new JobPosition { JobPositionId = 1, Name = "Chief Financial Officer (CFO)", Department = "Finance", PayGradeId = 10, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 2, Name = "Finance Manager", Department = "Finance", PayGradeId = 9, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 3, Name = "Senior Accountant", Department = "Finance", PayGradeId = 8, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 4, Name = "Accountant", Department = "Finance", PayGradeId = 7, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 5, Name = "Accounts Payable Officer", Department = "Finance", PayGradeId = 3, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 6, Name = "Accounts Receivable Officer", Department = "Finance", PayGradeId = 3, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 7, Name = "Payroll Officer", Department = "Finance", PayGradeId = 6, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 8, Name = "Finance Administrator", Department = "Finance", PayGradeId = 2, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 9, Name = "Billing Specialist", Department = "Finance", PayGradeId = 2, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 10, Name = "Accounts Assistant", Department = "Finance", PayGradeId = 1, AccessRole = "Employee" },

                // ===== HR =====
                new JobPosition { JobPositionId = 11, Name = "HR Manager", Department = "HR", PayGradeId = 8, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 12, Name = "Senior HR Advisor", Department = "HR", PayGradeId = 7, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 13, Name = "HR Advisor", Department = "HR", PayGradeId = 3, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 14, Name = "HR Coordinator", Department = "HR", PayGradeId = 2, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 15, Name = "HR Administrator", Department = "HR", PayGradeId = 1, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 16, Name = "Recruitment Specialist", Department = "HR", PayGradeId = 6, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 17, Name = "Talent Acquisition Coordinator", Department = "HR", PayGradeId = 3, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 18, Name = "Training & Development Officer", Department = "HR", PayGradeId = 6, AccessRole = "Admin" },

                // ===== IT =====
                new JobPosition { JobPositionId = 19, Name = "IT Manager", Department = "IT", PayGradeId = 8, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 20, Name = "Systems Administrator", Department = "IT", PayGradeId = 3, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 21, Name = "Network Administrator", Department = "IT", PayGradeId = 6, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 22, Name = "Software Developer", Department = "IT", PayGradeId = 6, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 23, Name = "Application Support Analyst", Department = "IT", PayGradeId = 3, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 24, Name = "IT Support Technician", Department = "IT", PayGradeId = 2, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 25, Name = "Helpdesk Support", Department = "IT", PayGradeId = 1, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 26, Name = "Database Administrator (DBA)", Department = "IT", PayGradeId = 6, AccessRole = "Employee" },

                // ===== OPERATIONS =====
                new JobPosition { JobPositionId = 27, Name = "Operations Manager", Department = "Operations", PayGradeId = 8, AccessRole = "Admin" },
                new JobPosition { JobPositionId = 28, Name = "Team Leader – Operations", Department = "Operations", PayGradeId = 4, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 29, Name = "Supervisor – Operations", Department = "Operations", PayGradeId = 5, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 30, Name = "Senior Officer – Operations", Department = "Operations", PayGradeId = 3, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 31, Name = "Office Administrator", Department = "Operations", PayGradeId = 2, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 32, Name = "Customer Service Representative", Department = "Operations", PayGradeId = 2, AccessRole = "Employee" },
                new JobPosition { JobPositionId = 33, Name = "Data Entry Operator", Department = "Operations", PayGradeId = 1, AccessRole = "Employee" }
            );
        }

        private void SeedHolidays(ModelBuilder b)
        {
            b.Entity<Holiday>().HasData(
                new Holiday { HolidayId = 1, Name = "New Year's Day", HolidayDate = new DateTime(2025, 1, 1), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 2, Name = "Day After New Year's Day", HolidayDate = new DateTime(2025, 1, 2), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 3, Name = "Waitangi Day", HolidayDate = new DateTime(2025, 2, 6), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 4, Name = "Good Friday", HolidayDate = new DateTime(2025, 4, 18), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 5, Name = "Easter Monday", HolidayDate = new DateTime(2025, 4, 21), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 6, Name = "ANZAC Day", HolidayDate = new DateTime(2025, 4, 25), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 7, Name = "King's Birthday", HolidayDate = new DateTime(2025, 6, 2), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 8, Name = "Matariki", HolidayDate = new DateTime(2025, 6, 20), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 9, Name = "Labour Day", HolidayDate = new DateTime(2025, 10, 27), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 10, Name = "Christmas Day", HolidayDate = new DateTime(2025, 12, 25), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 11, Name = "Boxing Day", HolidayDate = new DateTime(2025, 12, 26), HolidayType = "Public", IsPaidHoliday = true }
            );
        }

        private void SeedPayrollPeriods(ModelBuilder b)
        {
            b.Entity<PayrollPeriod>().HasData(
                new PayrollPeriod
                {
                    PayrollPeriodId = 1,
                    PeriodCode = "2025-M11",
                    PeriodStart = new DateTime(2025, 11, 1),
                    PeriodEnd = new DateTime(2025, 11, 30),
                    Closed = false
                },
                new PayrollPeriod
                {
                    PayrollPeriodId = 2,
                    PeriodCode = "2025-M12",
                    PeriodStart = new DateTime(2025, 12, 1),
                    PeriodEnd = new DateTime(2025, 12, 31),
                    Closed = false
                },
                new PayrollPeriod
                {
                    PayrollPeriodId = 3,
                    PeriodCode = "2025-FN13",
                    PeriodStart = new DateTime(2025, 11, 1),
                    PeriodEnd = new DateTime(2025, 11, 14),
                    Closed = true,
                    TotalAmount = 90000m
                }
            );
        }

        private void SeedEmployees(ModelBuilder b)
        {
            b.Entity<Employee>().HasData(

                // ===== TEMP ADMIN ACCOUNT =====
                new Employee
                {
                    EmployeeId = 1001,
                    FirstName = "Temp",
                    LastName = "Admin",
                    Email = "admin@nzftc.local",
                    Department = "HR",
                    JobPositionId = 11,     // HR Manager
                    PayGradeId = 8,
                    EmployeeCode = "NZFTC1001",
                    StartDate = new DateTime(2025, 11, 20),
                    Birthday = new DateTime(1990, 1, 1),
                    Gender = "Other",
                    Address = "N/A",
                    PasswordHash = new byte[0],  // You can replace with real hash
                    PasswordSalt = new byte[0]
                },
                new Employee
                {
                    EmployeeId = 1003,
                    FirstName = "Sarah",
                    LastName = "Williams",
                    Email = "sarah@nzftc.local",
                    Department = "IT",
                    JobPositionId = 22,  // Software Developer
                    PayGradeId = 6,
                    EmployeeCode = "NZFTC1003",
                    StartDate = new DateTime(2025, 11, 10),
                    Birthday = new DateTime(1997, 3, 12),
                    Gender = "Female",
                    Address = "42 Eden Terrace",
                    PasswordHash = new byte[0],
                    PasswordSalt = new byte[0]
                },
                new Employee
                {
                    EmployeeId = 1004,
                    FirstName = "Michael",
                    LastName = "Brown",
                    Email = "michael@nzftc.local",
                    Department = "Finance",
                    JobPositionId = 3,   // Senior Accountant
                    PayGradeId = 8,
                    EmployeeCode = "NZFTC1004",
                    StartDate = new DateTime(2025, 10, 5),
                    Birthday = new DateTime(1988, 9, 14),
                    Gender = "Male",
                    Address = "19 Queen Street",
                    PasswordHash = new byte[0],
                    PasswordSalt = new byte[0]
                },
                // ===== SAMPLE EMPLOYEE #1 =====
                new Employee
                {
                    EmployeeId = 1002,
                    FirstName = "TEMP",
                    LastName = "Emp",
                    Email = "emp@nzftc.local",
                    Department = "Finance",
                    JobPositionId = 4,     // Accountant
                    PayGradeId = 7,
                    EmployeeCode = "NZFTC1002",
                    StartDate = new DateTime(2025, 11, 20),
                    Birthday = new DateTime(1995, 5, 15),
                    Gender = "Male",
                    Address = "123 Finance Street",
                    PasswordHash = new byte[0],
                    PasswordSalt = new byte[0]
                }
            );
        }

        private void SeedEmployeeContacts(ModelBuilder b)
        {
            b.Entity<EmployeeEmergencyContact>().HasData(

                new EmployeeEmergencyContact
                {
                    EmergencyContactId = 1,
                    EmployeeId = 1001,
                    FullName = "Admin Contact",
                    Phone = "0000",
                    Email = "none@local"
                },

                new EmployeeEmergencyContact
                {
                    EmergencyContactId = 2,
                    EmployeeId = 1002,
                    FullName = "Jane Doe",
                    Phone = "0211234567",
                    Email = "jane.doe@example.com"
                }
            );
        }

        private void SeedLeaveBalances(ModelBuilder b)
        {
            b.Entity<EmployeeLeaveBalance>().HasData(
                new EmployeeLeaveBalance
                {
                    EmployeeLeaveBalanceId = 1,
                    EmployeeId = 1001,
                    AnnualAccrued = 0,
                    AnnualUsed = 0,
                    SickAccrued = 0,
                    SickUsed = 0,
                    UpdatedAt = new DateTime(2025, 1, 1)
                },
                new EmployeeLeaveBalance
                {
                    EmployeeLeaveBalanceId = 3,
                    EmployeeId = 1003,
                    AnnualAccrued = 5,
                    AnnualUsed = 0,
                    SickAccrued = 2,
                    SickUsed = 0,
                    UpdatedAt = new DateTime(2025, 1, 1)
                }, new EmployeeLeaveBalance
                {
                    EmployeeLeaveBalanceId = 4,
                    EmployeeId = 1004,
                    AnnualAccrued = 10,
                    AnnualUsed = 2,
                    SickAccrued = 5,
                    SickUsed = 1,
                    UpdatedAt = new DateTime(2025, 1, 1)
                },

                new EmployeeLeaveBalance
                {
                    EmployeeLeaveBalanceId = 2,
                    EmployeeId = 1002,
                    AnnualAccrued = 0,
                    AnnualUsed = 0,
                    SickAccrued = 0,
                    SickUsed = 0,
                    UpdatedAt = new DateTime(2025, 1, 1)
                }
            );
        }

        private void SeedSupportTickets(ModelBuilder b)
        {
            b.Entity<SupportTicket>().HasData(
                new SupportTicket
                {
                    Id = 1,
                    Subject = "Welcome to Support",
                    Message = "This is the initial seeded support ticket.",
                    Status = 0,
                    Priority = 0,
                    EmployeeId = 1002,      // created by sample employee
                    AssignedToId = 1001,    // assigned to admin
                    CreatedAt = new DateTime(2025, 11, 20),
                    UpdatedAt = null
                }
            );
        }

        private void SeedSupportMessages(ModelBuilder b)
        {
            b.Entity<SupportMessage>().HasData(

                new SupportMessage
                {
                    Id = 1,
                    TicketId = 1,
                    SenderEmployeeId = 1002,
                    SenderIsAdmin = false,
                    Body = "Hi, I need help setting up my account.",
                    SentAt = new DateTime(2025, 11, 20, 9, 0, 0)
                },

                new SupportMessage
                {
                    Id = 2,
                    TicketId = 1,
                    SenderEmployeeId = 1001,
                    SenderIsAdmin = true,
                    Body = "Admin here — your account is now active!",
                    SentAt = new DateTime(2025, 11, 20, 9, 5, 0)
                }
            );
        }

        private void SeedLeavePolicies(ModelBuilder b)
        {
            b.Entity<LeavePolicy>().HasData(
                new LeavePolicy
                {
                    LeavePolicyId = 1,
                    AnnualDefault = 20,
                    AnnualAccrualRate = 1.67m,
                    AnnualCarryOverLimit = 5,
                    AllowNegativeAnnual = false,

                    SickDefault = 10,
                    SickAccrualRate = 0,
                    AllowUnpaidSick = true,

                    CustomLeaveTypesJson = "[]",
                    UpdatedAt = new DateTime(2025, 1, 1)
                }
            );
        }
        private void SeedEmployeePayrollSummaries(ModelBuilder b)
        {
            b.Entity<EmployeePayrollSummary>().HasData(
                new EmployeePayrollSummary
                {
                    EmployeePayrollSummaryId = 1,
                    EmployeeId = 1001,
                    PayrollPeriodId = 1,
                    PayRate = 28.00m,
                    RateType = RateType.Hourly,
                    GrossPay = 5000m,
                    Deductions = 900m,
                    PAYE = 700m,
                    KiwiSaverEmployee = 150m,
                    KiwiSaverEmployer = 150m,
                    ACCLevy = 50m,
                    StudentLoan = 50m,
                    Status = PayrollSummaryStatus.Finalized
                },
                new EmployeePayrollSummary
                {
                    EmployeePayrollSummaryId = 2,
                    EmployeeId = 1002,
                    PayrollPeriodId = 1,
                    PayRate = 32.00m,
                    RateType = RateType.Hourly,
                    GrossPay = 6000m,
                    Deductions = 1000m,
                    PAYE = 750m,
                    KiwiSaverEmployee = 180m,
                    KiwiSaverEmployer = 180m,
                    ACCLevy = 60m,
                    StudentLoan = 30m,
                    Status = PayrollSummaryStatus.Paid
                }
            );
        }

        private void SeedPayrollSettings(ModelBuilder b)
        {
            b.Entity<PayrollSettings>().HasData(
                new PayrollSettings
                {
                    PayrollSettingsId = 1,
                    KiwiSaverEmployeePercent = 3.0m,
                    KiwiSaverEmployerPercent = 3.0m,
                    ACCLevyPercent = 1.53m,
                    EnableStudentLoan = true,
                    RegularHoursPerWeek = 40,
                    OvertimeMultiplier = 1.5m,
                    UpdatedAt = new DateTime(2025, 1, 1)
                }
            );
        } 

        private void SeedCalendarEvents(ModelBuilder b)
        {
            b.Entity<CalendarEvent>().HasData(
                new CalendarEvent
                {
                    Id = 1,
                    Title = "System Go-Live",
                    Description = "NZFTC EMS officially launched!",
                    Start = new DateTime(2025, 11, 20, 9, 0, 0),
                    End = new DateTime(2025, 11, 20, 17, 0, 0),
                    EventType = Data.Entities.CalendarEventType.Other,
                    OwnerUsername = "System"
                }
            );
        }
    }
}
