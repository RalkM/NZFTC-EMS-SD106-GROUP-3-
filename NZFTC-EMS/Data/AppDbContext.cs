using Microsoft.EntityFrameworkCore;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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

        // Calendar module
        public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        // Support module
        public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
        public DbSet<SupportMessage> SupportMessages => Set<SupportMessage>();
=======
public DbSet<Announcement> Announcements { get; set; } = default!;
>>>>>>> Stashed changes
=======
public DbSet<Announcement> Announcements { get; set; } = default!;
>>>>>>> Stashed changes
=======
public DbSet<Announcement> Announcements { get; set; } = default!;
>>>>>>> Stashed changes


        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // ======================
            //  PAYGRADE SEED DATA
            // ======================
            b.Entity<PayGrade>().HasData(
                new PayGrade { PayGradeId = 1, Name = "PG-minimum", Description = "Minimum Pay", BaseRate = 23.50m, RateType = (RateType)0, IsActive = true },
                new PayGrade { PayGradeId = 2, Name = "PG-REG", Description = "Regular Staff", BaseRate = 28.00m, RateType = (RateType)0, IsActive = true },
                new PayGrade { PayGradeId = 3, Name = "PG-SSTAFF", Description = "Senior Staff", BaseRate = 32.00m, RateType = (RateType)0, IsActive = true },
                new PayGrade { PayGradeId = 4, Name = "PG-TL", Description = "Team Leader", BaseRate = 38.00m, RateType = (RateType)0, IsActive = true },
                new PayGrade { PayGradeId = 5, Name = "PG-SUP", Description = "Supervisor", BaseRate = 45.00m, RateType = (RateType)0, IsActive = true },
                new PayGrade { PayGradeId = 6, Name = "PG-SPEC", Description = "Specialist", BaseRate = 50.00m, RateType = (RateType)0, IsActive = true },
                new PayGrade { PayGradeId = 7, Name = "PG-SAL-JM", Description = "Salary - Junior/Assistant Manager", BaseRate = 65000.00m, RateType = (RateType)1, IsActive = true },
                new PayGrade { PayGradeId = 8, Name = "PG-SAL-MAN", Description = "Salary - Department Manager", BaseRate = 80000.00m, RateType = (RateType)1, IsActive = true },
                new PayGrade { PayGradeId = 9, Name = "PG-SAL-SRMAN", Description = "Salary - Senior Manager", BaseRate = 95000.00m, RateType = (RateType)1, IsActive = true },
                new PayGrade { PayGradeId = 10, Name = "PG-SAL-GM", Description = "Salary - General Manager", BaseRate = 100000.00m, RateType = (RateType)1, IsActive = true }
            );

            // ======================
            //  JOB POSITION SEED DATA
            // ======================
            b.Entity<JobPosition>().HasData(
                new JobPosition { JobPositionId = 1, Name = "Chief Financial Officer (CFO)", Department = "Finance", PayGradeId = 10, AccessRole = "Admin", Description = "Top-level finance leadership", IsActive = true },
                new JobPosition { JobPositionId = 2, Name = "Finance Manager", Department = "Finance", PayGradeId = 9, AccessRole = "Admin", Description = "Leads the finance team and reporting", IsActive = true },
                new JobPosition { JobPositionId = 3, Name = "Senior Accountant", Department = "Finance", PayGradeId = 8, AccessRole = "Employee", Description = "Handles complex accounting and reporting", IsActive = true },
                new JobPosition { JobPositionId = 4, Name = "Accountant", Department = "Finance", PayGradeId = 7, AccessRole = "Employee", Description = "General accounting duties", IsActive = true },
                new JobPosition { JobPositionId = 5, Name = "Accounts Payable Officer", Department = "Finance", PayGradeId = 3, AccessRole = "Employee", Description = "Manages supplier invoices and payments", IsActive = true },
                new JobPosition { JobPositionId = 6, Name = "Accounts Receivable Officer", Department = "Finance", PayGradeId = 3, AccessRole = "Employee", Description = "Manages customer invoicing and collections", IsActive = true },
                new JobPosition { JobPositionId = 7, Name = "Payroll Officer", Department = "Finance", PayGradeId = 6, AccessRole = "Employee", Description = "Processes staff payroll", IsActive = true },
                new JobPosition { JobPositionId = 8, Name = "Finance Administrator", Department = "Finance", PayGradeId = 2, AccessRole = "Employee", Description = "Provides general admin support to finance", IsActive = true },
                new JobPosition { JobPositionId = 9, Name = "Billing Specialist", Department = "Finance", PayGradeId = 2, AccessRole = "Employee", Description = "Prepares and manages billing", IsActive = true },
                new JobPosition { JobPositionId = 10, Name = "Accounts Assistant", Department = "Finance", PayGradeId = 1, AccessRole = "Employee", Description = "Entry-level support in finance", IsActive = true },

                new JobPosition { JobPositionId = 11, Name = "HR Manager", Department = "HR", PayGradeId = 8, AccessRole = "Admin", Description = "Leads HR operations and strategy", IsActive = true },
                new JobPosition { JobPositionId = 12, Name = "Senior HR Advisor", Department = "HR", PayGradeId = 7, AccessRole = "Admin", Description = "Senior advisory role in HR", IsActive = true },
                new JobPosition { JobPositionId = 13, Name = "HR Advisor", Department = "HR", PayGradeId = 3, AccessRole = "Admin", Description = "Generalist HR support", IsActive = true },
                new JobPosition { JobPositionId = 14, Name = "HR Coordinator", Department = "HR", PayGradeId = 2, AccessRole = "Admin", Description = "Coordinates HR processes and documentation", IsActive = true },
                new JobPosition { JobPositionId = 15, Name = "HR Administrator", Department = "HR", PayGradeId = 1, AccessRole = "Admin", Description = "Admin support across HR functions", IsActive = true },
                new JobPosition { JobPositionId = 16, Name = "Recruitment Specialist", Department = "HR", PayGradeId = 6, AccessRole = "Admin", Description = "Manages recruitment and selection", IsActive = true },
                new JobPosition { JobPositionId = 17, Name = "Talent Acquisition Coordinator", Department = "HR", PayGradeId = 3, AccessRole = "Admin", Description = "Supports talent acquisition activities", IsActive = true },
                new JobPosition { JobPositionId = 18, Name = "Training & Development Officer", Department = "HR", PayGradeId = 6, AccessRole = "Admin", Description = "Coordinates training and staff development", IsActive = true },

                new JobPosition { JobPositionId = 19, Name = "IT Manager", Department = "IT", PayGradeId = 8, AccessRole = "Admin", Description = "Leads IT operations and projects", IsActive = true },
                new JobPosition { JobPositionId = 20, Name = "Systems Administrator", Department = "IT", PayGradeId = 3, AccessRole = "Employee", Description = "Maintains servers and systems", IsActive = true },
                new JobPosition { JobPositionId = 21, Name = "Network Administrator", Department = "IT", PayGradeId = 6, AccessRole = "Employee", Description = "Manages network infrastructure", IsActive = true },
                new JobPosition { JobPositionId = 22, Name = "Software Developer", Department = "IT", PayGradeId = 6, AccessRole = "Employee", Description = "Develops and maintains software applications", IsActive = true },
                new JobPosition { JobPositionId = 23, Name = "Application Support Analyst", Department = "IT", PayGradeId = 3, AccessRole = "Employee", Description = "Supports business applications", IsActive = true },
                new JobPosition { JobPositionId = 24, Name = "IT Support Technician", Department = "IT", PayGradeId = 2, AccessRole = "Employee", Description = "First-line IT support", IsActive = true },
                new JobPosition { JobPositionId = 25, Name = "Helpdesk Support", Department = "IT", PayGradeId = 1, AccessRole = "Employee", Description = "Handles basic IT helpdesk requests", IsActive = true },
                new JobPosition { JobPositionId = 26, Name = "Database Administrator (DBA)", Department = "IT", PayGradeId = 6, AccessRole = "Employee", Description = "Manages databases and performance", IsActive = true },

                new JobPosition { JobPositionId = 27, Name = "Operations Manager", Department = "Operations", PayGradeId = 8, AccessRole = "Admin", Description = "Oversees day-to-day operations", IsActive = true },
                new JobPosition { JobPositionId = 28, Name = "Team Leader – Operations", Department = "Operations", PayGradeId = 4, AccessRole = "Employee", Description = "Leads an operations team", IsActive = true },
                new JobPosition { JobPositionId = 29, Name = "Supervisor – Operations", Department = "Operations", PayGradeId = 5, AccessRole = "Employee", Description = "Supervises operational staff", IsActive = true },
                new JobPosition { JobPositionId = 30, Name = "Senior Officer – Operations", Department = "Operations", PayGradeId = 3, AccessRole = "Employee", Description = "Senior operations officer role", IsActive = true },
                new JobPosition { JobPositionId = 31, Name = "Office Administrator", Department = "Operations", PayGradeId = 2, AccessRole = "Employee", Description = "General office administration", IsActive = true },
                new JobPosition { JobPositionId = 32, Name = "Customer Service Representative", Department = "Operations", PayGradeId = 2, AccessRole = "Employee", Description = "Frontline customer service", IsActive = true },
                new JobPosition { JobPositionId = 33, Name = "Data Entry Operator", Department = "Operations", PayGradeId = 1, AccessRole = "Employee", Description = "Data entry and basic admin tasks", IsActive = true }
            );
            // ======================
            //   HOLIDAY SEED DATA
            // ======================
            b.Entity<Holiday>().HasData(
                new Holiday { HolidayId = 1, Name = "New Year's Day", HolidayDate = new DateTime(2025, 1, 1), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 2, Name = "Waitangi Day", HolidayDate = new DateTime(2025, 2, 6), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 3, Name = "Good Friday", HolidayDate = new DateTime(2025, 4, 18), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 4, Name = "Easter Monday", HolidayDate = new DateTime(2025, 4, 21), HolidayType = "Public", IsPaidHoliday = true },
                new Holiday { HolidayId = 5, Name = "ANZAC Day", HolidayDate = new DateTime(2025, 4, 25), HolidayType = "Public", IsPaidHoliday = true }
            );



<<<<<<< Updated upstream
            // ======================
            //   PAYROLL PERIOD SEED
            // ======================
            b.Entity<PayrollPeriod>().HasData(
                new PayrollPeriod
                {
                    PayrollPeriodId = 1,
                    PeriodCode = "2025-11-M1",
                    PeriodStart = new DateTime(2025, 11, 1),
                    PeriodEnd = new DateTime(2025, 11, 30),
                    TotalAmount = 0.00m,
                    Closed = false
                }
            ); // ===== SEED BLOCK 5: EmployeeLeaveBalances =====
            b.Entity<EmployeeLeaveBalance>().HasData(
                new EmployeeLeaveBalance
                {
                    EmployeeLeaveBalanceId = 1,
                    EmployeeId = 1001,           // TEMP ADMIN
                    AnnualAccrued = 0,
                    AnnualUsed = 0,
                    SickAccrued = 0,
                    SickUsed = 0,
                    CarryOverAnnual = 0,
                    UpdatedAt = new DateTime(2025, 1, 1)
                },
                new EmployeeLeaveBalance
                {
                    EmployeeLeaveBalanceId = 2,
                    EmployeeId = 1002,           // TEMP EMPLOYEE
                    AnnualAccrued = 0,
                    AnnualUsed = 0,
                    SickAccrued = 0,
                    SickUsed = 0,
                    CarryOverAnnual = 0,
                    UpdatedAt = new DateTime(2025, 1, 1)
                }
            ); // ===== SEED BLOCK 6: EmployeeEmergencyContacts =====
            b.Entity<EmployeeEmergencyContact>().HasData(
                new EmployeeEmergencyContact
                {
                    EmergencyContactId = 1,
                    EmployeeId = 1001,
                    FullName = "Temp Admin Contact",
                    Relationship = "N/A",
                    Phone = "0000",
                    Email = "none@local"
                },
                new EmployeeEmergencyContact
                {
                    EmergencyContactId = 2,
                    EmployeeId = 1002,
                    FullName = "Temp Employee Contact",
                    Relationship = "N/A",
                    Phone = "0000",
                    Email = "none@local"
                }
            ); // ===== SEED BLOCK 7: Employees (TEMP ONLY) =====
            b.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1001,
                    FirstName = "Temp",
                    LastName = "Admin",
                    Email = "admin@nzftc.local",
                    PasswordHash = new byte[] { },
                    PasswordSalt = new byte[] { },
                    JobPositionId = 11,     // HR Manager (Admin role)
                    PayGradeId = 8,
                    StartDate = new DateTime(2025, 11, 20),
                    Department = "HR",
                    EmployeeCode = "TEMP001",
                    Gender = "N/A",
                    Address = "N/A",
                    Birthday = null
                },
                new Employee
                {
                    EmployeeId = 1002,
                    FirstName = "Temp",
                    LastName = "Employee",
                    Email = "emp@nzftc.local",
                    PasswordHash = new byte[] { },
                    PasswordSalt = new byte[] { },
                    JobPositionId = 31,     // Office Administrator
                    PayGradeId = 2,
                    StartDate = new DateTime(2025, 11, 20),
                    Department = "Operations",
                    EmployeeCode = "TEMP002",
                    Gender = "N/A",
                    Address = "N/A",
                    Birthday = null
                }
            );// ===== SEED BLOCK 8: SupportTickets =====
            b.Entity<SupportTicket>().HasData(
                new SupportTicket
                {
                    Id = 1,
                    Subject = "Welcome to Support",
                    Message = "This is a sample seeded ticket.",
                    Status = 0,
                    Priority = 0,
                    EmployeeId = 1002,      // temp employee created the ticket
                    AssignedToId = 1001,    // temp admin assigned
                    CreatedAt = new DateTime(2025, 1, 1),
                    UpdatedAt = null        // only allowed if property is nullable
                }
            );// ===== SEED BLOCK 9: SupportMessages =====
            b.Entity<SupportMessage>().HasData(
                new SupportMessage
                {
                    Id = 1,
                    TicketId = 1,
                    SenderEmployeeId = 1002,
                    Body = "Hi, I need help setting up my account.",
                    SentAt = new DateTime(2025, 1, 1)
                },
                new SupportMessage
                {
                    Id = 2,
                    TicketId = 1,
                    SenderEmployeeId = 1001,
                    Body = "Admin here — your account is now active!",
                    SentAt = new DateTime(2025, 1, 1)
                }
            );// ===== SEED BLOCK 10: CalendarEvents =====
            b.Entity<CalendarEvent>().HasData(
                new CalendarEvent
                {
                    Id = 1,
                    Title = "System Go-Live",
                    Description = "NZFTC EMS officially launched!",
                    Start = new DateTime(2025, 11, 20, 9, 0, 0),
                    End = new DateTime(2025, 11, 20, 17, 0, 0),
                    EventType = CalendarEventType.Other,
                    OwnerUsername = "System"
                }
            );
=======

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

// 🔹 NEW
SeedPayrollRuns(b);
SeedEmployeePayrollSummaries(b);
SeedAnnouncements(b);
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes



<<<<<<< Updated upstream
            // ======================
            //   JOB POSITION (SQL ACCURATE)
            // ======================
=======
        // ==========================================================
        //                    CONFIGURATION BLOCKS
        // ==========================================================

        private void SeedAnnouncements(ModelBuilder b)
{
    b.Entity<Announcement>().HasData(
        new Announcement
        {
            Id    = 1,
            Title = "Welcome to NZFTC EMS",
            Body  = "Remember to complete your profile and update emergency contacts.",

            // HasData needs a constant, not DateTime.UtcNow
            CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
            IsActive  = true
        });
}
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

private void SeedEmployeePayrollSummaries(ModelBuilder b)
{
    b.Entity<EmployeePayrollSummary>().HasData(
        // Weekly run for Temp Admin (EmployeeId = 1001)
        new EmployeePayrollSummary
        {
            EmployeePayrollSummaryId = 1,
            EmployeeId      = 1001,   // Temp Admin from SeedEmployees
            PayrollPeriodId = 1,      // existing period (2025-M11)
            PayrollRunId    = 1,      // weekly run above

            PayRate   = 50.00m,
            RateType  = RateType.Hourly,
            TotalHours= 40m,
            GrossPay  = 2000m,

            PAYE              = 200m,
            KiwiSaverEmployee = 45m,
            KiwiSaverEmployer = 45m,
            ACCLevy           = 15m,
            StudentLoan       = 0m,
            Deductions        = 260m,     // 200 + 45 + 15

            Status      = PayrollSummaryStatus.Paid,
            GeneratedAt = new DateTime(2025, 11, 13),
            NetPay = 1740m
        },

        // Monthly run for another seeded employee (e.g. 1003)
        new EmployeePayrollSummary
        {
            EmployeePayrollSummaryId = 2,
            EmployeeId      = 1001,   // Sarah / another seeded employee
            PayrollPeriodId = 1,      // 2025-M11
            PayrollRunId    = 2,      // monthly run above

            PayRate   = 50.00m,
            RateType  = RateType.Hourly,
            TotalHours= 40m,
            GrossPay  = 2000m,

             PAYE              = 200m,
            KiwiSaverEmployee = 45m,
            KiwiSaverEmployer = 45m,
            ACCLevy           = 15m,
            StudentLoan       = 0m,
            Deductions        = 260m,     // 200 + 45 + 15

            Status      = PayrollSummaryStatus.Paid,
            GeneratedAt = new DateTime(2025, 11, 20),
            NetPay = 1740m
        }
    );
}


private void SeedPayrollRuns(ModelBuilder b)
{
    b.Entity<PayrollRun>().HasData(
        // Example: a past weekly run
        new PayrollRun
        {
            PayrollRunId = 1,
            PeriodStart  = new DateTime(2025, 11, 6), // Thu
            PeriodEnd    = new DateTime(2025, 11, 12), // Wed
            PayFrequency = PayFrequency.Weekly,
            Status       = PayrollRunStatus.Paid,
            CreatedAt    = new DateTime(2025, 11, 20),
            ProcessedAt  = new DateTime(2025, 11, 20),
            PaidAt       = new DateTime(2025, 11, 20)
        },

        // Example: a monthly run
        new PayrollRun
        {
            PayrollRunId = 2,
            PeriodStart  = new DateTime(2025, 11, 13),
            PeriodEnd    = new DateTime(2025, 11, 19),
            PayFrequency = PayFrequency.Weekly,
            Status       = PayrollRunStatus.Finalizing,
            CreatedAt    = new DateTime(2025, 11, 30),
            ProcessedAt  = null,
            PaidAt       = null
        }
    );
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
>>>>>>> Stashed changes
            b.Entity<JobPosition>(e =>
            {
                e.ToTable("jobpositions");

                e.HasKey(x => x.JobPositionId);

                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();

                e.Property(x => x.Department).HasMaxLength(80);
                e.Property(x => x.AccessRole).HasMaxLength(20);
                e.Property(x => x.Description).HasMaxLength(400);

                e.Property(x => x.IsActive).IsRequired();

                // 🔹 Tie FK + navigation together so EF only makes ONE relationship
                e.HasOne(x => x.PayGrade)
                .WithMany(pg => pg.JobPositions)
                .HasForeignKey(x => x.PayGradeId)
                .OnDelete(DeleteBehavior.Restrict);
            });



            // ======================
            //   PAYGRADE (SQL ACCURATE)
            // ======================
            b.Entity<PayGrade>(e =>
            {
                e.ToTable("paygrades");

                e.HasKey(x => x.PayGradeId);

                e.Property(x => x.Name).HasMaxLength(50).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();

                e.Property(x => x.Description).HasMaxLength(255);
                e.Property(x => x.BaseRate).HasPrecision(12, 2);
                e.Property(x => x.RateType);
                e.Property(x => x.IsActive);
            });


            // ======================
            //   EMPLOYEE (SQL ACCURATE)
            // ======================
            b.Entity<Employee>(e =>
            {
                e.ToTable("employees");

                e.HasKey(x => x.EmployeeId);

                e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
                e.Property(x => x.LastName).HasMaxLength(100).IsRequired();

                e.Property(x => x.Birthday).HasColumnType("datetime(6)");
                e.Property(x => x.Gender).HasMaxLength(20);
                e.Property(x => x.Address).HasMaxLength(300);
                e.Property(x => x.Phone).HasMaxLength(30);

                e.Property(x => x.Email).HasMaxLength(255).IsRequired();
                e.HasIndex(x => x.Email).IsUnique();

                e.Property(x => x.EmployeeCode).HasMaxLength(20);
                e.HasIndex(x => x.EmployeeCode).IsUnique();

                e.Property(x => x.StartDate).HasColumnType("date");
                e.Property(x => x.Department).HasMaxLength(80);

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


            // ======================
            //   EMERGENCY CONTACTS
            // ======================
            b.Entity<EmployeeEmergencyContact>(e =>
            {
                e.ToTable("employeeemergencycontacts");

                e.HasKey(x => x.EmergencyContactId);

                e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
                e.Property(x => x.Relationship).HasMaxLength(100);
                e.Property(x => x.Phone).HasMaxLength(30);
                e.Property(x => x.Email).HasMaxLength(255);

                e.HasOne(x => x.Employee)
                    .WithMany(e => e.EmergencyContacts)
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // ======================
            //   LEAVE REQUESTS
            // ======================
            b.Entity<LeaveRequest>(e =>
            {
                e.ToTable("leaverequests");

                e.HasKey(x => x.LeaveRequestId);

                e.Property(x => x.LeaveType).HasMaxLength(50).IsRequired();
                e.Property(x => x.StartDate).HasColumnType("date");
                e.Property(x => x.EndDate).HasColumnType("date");
                e.Property(x => x.Reason).HasMaxLength(500);

                e.HasIndex(x => new { x.StartDate, x.EndDate });

                e.HasOne(x => x.Employee)
                    .WithMany(e => e.LeaveRequests)
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne<Employee>()
                    .WithMany()
                    .HasForeignKey(x => x.ApprovedByEmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // ======================
            //   PAYROLL PERIOD
            // ======================
            b.Entity<PayrollPeriod>(e =>
            {
                e.ToTable("payrollperiods");

                e.HasKey(x => x.PayrollPeriodId);

                e.Property(x => x.PeriodCode).HasMaxLength(50).IsRequired();
                e.HasIndex(x => x.PeriodCode).IsUnique();

                e.Property(x => x.PeriodStart).HasColumnType("date");
                e.Property(x => x.PeriodEnd).HasColumnType("date");

                e.Property(x => x.TotalAmount).HasPrecision(14, 2);
            });


            // ======================
            //   PAYROLL SUMMARY
            // ======================
            b.Entity<EmployeePayrollSummary>(e =>
            {
                e.ToTable("employeepayrollsummaries");

                e.HasKey(x => x.EmployeePayrollSummaryId);

                e.HasIndex(x => new { x.PayrollPeriodId, x.EmployeeId }).IsUnique();

                e.Property(x => x.PayRate).HasPrecision(12, 2);
                e.Property(x => x.GrossPay).HasPrecision(14, 2);
                e.Property(x => x.Deductions).HasPrecision(14, 2);
                e.Property(x => x.NetPay).HasPrecision(14, 2)
                    .HasComputedColumnSql("(`GrossPay` - `Deductions`)", stored: true);

                e.HasOne(x => x.Employee)
                    .WithMany(e => e.PayrollSummaries)
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.PayrollPeriod)
                    .WithMany(p => p.Summaries)
                    .HasForeignKey(x => x.PayrollPeriodId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // ======================
            //   GRIEVANCES
            // ======================
            b.Entity<Grievance>(e =>
            {
                e.ToTable("grievances");

                e.HasKey(x => x.GrievanceId);

                e.Property(x => x.Subject).HasMaxLength(200).IsRequired();
                e.Property(x => x.EmployeeMessage).HasColumnType("longtext");

                e.HasOne(x => x.Employee)
                    .WithMany(e => e.Grievances)
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // ======================
            //   HOLIDAYS
            // ======================
            b.Entity<Holiday>(e =>
            {
                e.ToTable("holidays");

                e.HasKey(x => x.HolidayId);

                e.Property(x => x.Name).HasMaxLength(150).IsRequired();
                e.Property(x => x.HolidayDate).HasColumnType("date");
                e.HasIndex(x => x.HolidayDate).IsUnique();

                e.Property(x => x.HolidayType).HasMaxLength(50);
                e.Property(x => x.IsPaidHoliday);
            });


            // ======================
            //   SUPPORT MODULE
            // ======================
            b.Entity<SupportTicket>(e =>
            {
                e.ToTable("supporttickets");

                e.HasKey(x => x.Id);

                e.Property(x => x.Subject).HasMaxLength(120).IsRequired();
                e.Property(x => x.Message).HasMaxLength(4000).IsRequired();

                e.Property(x => x.Status).HasConversion<int>();
                e.Property(x => x.Priority).HasConversion<int>();

                e.HasMany(x => x.Messages)
                    .WithOne(m => m.Ticket)
                    .HasForeignKey(m => m.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            b.Entity<SupportMessage>(e =>
            {
                e.ToTable("supportmessages");

                e.HasKey(x => x.Id);
                e.Property(x => x.Body).HasMaxLength(4000).IsRequired();
            });


            // ======================
            //   EMPLOYEE LEAVE BALANCE
            // ======================
            b.Entity<EmployeeLeaveBalance>(e =>
            {
                e.ToTable("employeeleavebalances");

                e.HasKey(x => x.EmployeeLeaveBalanceId);

                e.Property(x => x.AnnualAccrued).HasPrecision(10, 2);
                e.Property(x => x.AnnualUsed).HasPrecision(10, 2);
                e.Property(x => x.SickAccrued).HasPrecision(10, 2);
                e.Property(x => x.SickUsed).HasPrecision(10, 2);
                e.Property(x => x.CarryOverAnnual).HasPrecision(10, 2);

                e.HasOne(x => x.Employee)
                    .WithMany(e => e.LeaveBalances)
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}