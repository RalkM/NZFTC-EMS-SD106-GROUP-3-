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

public DbSet<Announcement> Announcements { get; set; } = default!;

public DbSet<Grievance> Grievances => Set<Grievance>();
public SupportPriority Priority { get; set; }




        protected override void OnModelCreating(ModelBuilder b)
{
    base.OnModelCreating(b);

    // 1. CONFIGURE ALL ENTITIES FIRST
    ConfigureJobPosition(b);
    ConfigurePayGrade(b);

    ConfigureEmployee(b);                 // ✅ ADD THIS
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
    ConfigureEmployeePayrollSummary(b);   // ✅ ADD THIS

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

    // NEW
    SeedPayrollRuns(b);
    SeedEmployeePayrollSummaries(b);

    // NEW SEED BLOCKS WE’RE ADDING
    SeedLeaveRequests(b);
    SeedTimesheetEntries(b);
}

        // ==========================================================
        //                    CONFIGURATION BLOCKS
        // ==========================================================

private void SeedLeaveRequests(ModelBuilder b)
{
    b.Entity<LeaveRequest>().HasData(
        // Pending annual leave – TEMP Emp
        new LeaveRequest
        {
            LeaveRequestId       = 1,
            EmployeeId           = 1002,
            LeaveType            = "Annual",
            StartDate            = new DateTime(2025, 11, 25),
            EndDate              = new DateTime(2025, 11, 27),
            Reason               = "Family event",
            Status               = LeaveStatus.Pending,
            RequestedAt          = new DateTime(2025, 11, 20, 10, 0, 0),
            ApprovedByEmployeeId = null,
            ApprovedAt           = null
        },

        // Approved sick leave – Sarah
        new LeaveRequest
        {
            LeaveRequestId       = 2,
            EmployeeId           = 1003,
            LeaveType            = "Sick",
            StartDate            = new DateTime(2025, 11, 18),
            EndDate              = new DateTime(2025, 11, 19),
            Reason               = "Flu",
            Status               = LeaveStatus.Approved,
            RequestedAt          = new DateTime(2025, 11, 17, 9, 30, 0),
            ApprovedByEmployeeId = 1001,
            ApprovedAt           = new DateTime(2025, 11, 17, 14, 0, 0)
        },

        // Rejected annual leave – Michael
        new LeaveRequest
        {
            LeaveRequestId       = 3,
            EmployeeId           = 1004,
            LeaveType            = "Annual",
            StartDate            = new DateTime(2025, 12, 2),
            EndDate              = new DateTime(2025, 12, 4),
            Reason               = "Overlaps with year-end close",
            Status               = LeaveStatus.Rejected,
            RequestedAt          = new DateTime(2025, 11, 22, 11, 15, 0),
            ApprovedByEmployeeId = 1001,
            ApprovedAt           = new DateTime(2025, 11, 23, 16, 0, 0)
        },

        // Pending sick – Olivia
        new LeaveRequest
        {
            LeaveRequestId       = 4,
            EmployeeId           = 1005,
            LeaveType            = "Sick",
            StartDate            = new DateTime(2025, 11, 22),
            EndDate              = new DateTime(2025, 11, 22),
            Reason               = "Migraine",
            Status               = LeaveStatus.Pending,
            RequestedAt          = new DateTime(2025, 11, 21, 15, 0, 0),
            ApprovedByEmployeeId = null,
            ApprovedAt           = null
        },

        // Approved annual – Daniel
        new LeaveRequest
        {
            LeaveRequestId       = 5,
            EmployeeId           = 1006,
            LeaveType            = "Annual",
            StartDate            = new DateTime(2025, 12, 10),
            EndDate              = new DateTime(2025, 12, 11),
            Reason               = "Short break",
            Status               = LeaveStatus.Approved,
            RequestedAt          = new DateTime(2025, 11, 19, 10, 0, 0),
            ApprovedByEmployeeId = 1001,
            ApprovedAt           = new DateTime(2025, 11, 19, 16, 0, 0)
        }
    );
}


private void SeedGrievances(ModelBuilder b)
{
    b.Entity<Grievance>().HasData(
        // Open grievance from TEMP Emp
        new Grievance
        {
            GrievanceId      = 1,
            EmployeeId       = 1002, // TEMP Emp
            Subject          = "Roster concerns",
            SubmittedAt      = new DateTime(2025, 11, 19, 9, 0, 0),
            EmployeeMessage  = "My roster has been changed without notice and clashes with study.",
            AdminResponse    = null,
            Status           = GrievanceStatus.Open
        },

        // In review grievance from Sarah
        new Grievance
        {
            GrievanceId      = 2,
            EmployeeId       = 1003, // Sarah
            Subject          = "Equipment not working",
            SubmittedAt      = new DateTime(2025, 11, 18, 15, 30, 0),
            EmployeeMessage  = "My workstation keeps freezing and affects my productivity.",
            AdminResponse    = "IT has been notified and will replace your workstation this week.",
            Status           = GrievanceStatus.InReview
        },

        // Closed grievance from Michael
        new Grievance
        {
            GrievanceId      = 3,
            EmployeeId       = 1004, // Michael
            Subject          = "Payroll discrepancy – October",
            SubmittedAt      = new DateTime(2025, 11, 10, 14, 0, 0),
            EmployeeMessage  = "I believe my overtime for October was underpaid.",
            AdminResponse    = "We have recalculated and processed an adjustment in your next pay.",
            Status           = GrievanceStatus.Closed
        }
    );
}

private void SeedTimesheetEntries(ModelBuilder b)
{
    b.Entity<TimesheetEntry>().HasData(
        // Temp Admin – linked to run 1
        new TimesheetEntry
        {
            TimesheetEntryId = 1,
            EmployeeId       = 1001,
            WorkDate         = new DateTime(2025, 11, 7),
            StartTime        = new TimeSpan(9, 0, 0),
            BreakStartTime   = new TimeSpan(12, 0, 0),
            BreakEndTime     = new TimeSpan(12, 30, 0),
            FinishTime       = new TimeSpan(17, 0, 0),
            TotalHours       = 7.5m,
            Status           = TimesheetStatus.Approved,
            CreatedAt        = new DateTime(2025, 11, 7, 8, 30, 0),
            SubmittedAt      = new DateTime(2025, 11, 7, 17, 5, 0),
            ApprovedAt       = new DateTime(2025, 11, 8, 9, 0, 0),
            PayrollRunId     = 1,
            AdminNote        = "Approved – standard day."
        },

        // TEMP Emp – submitted
        new TimesheetEntry
        {
            TimesheetEntryId = 2,
            EmployeeId       = 1002,
            WorkDate         = new DateTime(2025, 11, 11),
            StartTime        = new TimeSpan(8, 30, 0),
            BreakStartTime   = new TimeSpan(12, 30, 0),
            BreakEndTime     = new TimeSpan(13, 0, 0),
            FinishTime       = new TimeSpan(17, 0, 0),
            TotalHours       = 8.0m,
            Status           = TimesheetStatus.Submitted,
            CreatedAt        = new DateTime(2025, 11, 11, 8, 0, 0),
            SubmittedAt      = new DateTime(2025, 11, 11, 17, 10, 0),
            ApprovedAt       = null,
            PayrollRunId     = 1,
            AdminNote        = null
        },

        // TEMP Emp – draft
        new TimesheetEntry
        {
            TimesheetEntryId = 3,
            EmployeeId       = 1002,
            WorkDate         = new DateTime(2025, 11, 12),
            StartTime        = new TimeSpan(9, 0, 0),
            BreakStartTime   = new TimeSpan(12, 30, 0),
            BreakEndTime     = new TimeSpan(13, 0, 0),
            FinishTime       = new TimeSpan(18, 0, 0),
            TotalHours       = 8.5m,
            Status           = TimesheetStatus.Draft,
            CreatedAt        = new DateTime(2025, 11, 12, 8, 45, 0),
            SubmittedAt      = null,
            ApprovedAt       = null,
            PayrollRunId     = null,
            AdminNote        = null
        },

        // Sarah – approved, run 1
        new TimesheetEntry
        {
            TimesheetEntryId = 4,
            EmployeeId       = 1003,
            WorkDate         = new DateTime(2025, 11, 10),
            StartTime        = new TimeSpan(9, 0, 0),
            BreakStartTime   = new TimeSpan(13, 0, 0),
            BreakEndTime     = new TimeSpan(13, 30, 0),
            FinishTime       = new TimeSpan(18, 0, 0),
            TotalHours       = 8.5m,
            Status           = TimesheetStatus.Approved,
            CreatedAt        = new DateTime(2025, 11, 10, 8, 40, 0),
            SubmittedAt      = new DateTime(2025, 11, 10, 18, 5, 0),
            ApprovedAt       = new DateTime(2025, 11, 11, 9, 30, 0),
            PayrollRunId     = 1,
            AdminNote        = "Approved – includes 0.5h overtime."
        },

        // Michael – rejected
        new TimesheetEntry
        {
            TimesheetEntryId = 5,
            EmployeeId       = 1004,
            WorkDate         = new DateTime(2025, 11, 9),
            StartTime        = new TimeSpan(10, 0, 0),
            BreakStartTime   = new TimeSpan(14, 0, 0),
            BreakEndTime     = new TimeSpan(14, 30, 0),
            FinishTime       = new TimeSpan(19, 0, 0),
            TotalHours       = 8.5m,
            Status           = TimesheetStatus.Rejected,
            CreatedAt        = new DateTime(2025, 11, 9, 9, 30, 0),
            SubmittedAt      = new DateTime(2025, 11, 9, 19, 10, 0),
            ApprovedAt       = new DateTime(2025, 11, 10, 10, 0, 0),
            PayrollRunId     = null,
            AdminNote        = "Incorrect break time; please resubmit."
        },

        // Olivia – approved, run 2
        new TimesheetEntry
        {
            TimesheetEntryId = 6,
            EmployeeId       = 1005,
            WorkDate         = new DateTime(2025, 11, 14),
            StartTime        = new TimeSpan(8, 30, 0),
            BreakStartTime   = new TimeSpan(12, 30, 0),
            BreakEndTime     = new TimeSpan(13, 0, 0),
            FinishTime       = new TimeSpan(17, 0, 0),
            TotalHours       = 8.0m,
            Status           = TimesheetStatus.Approved,
            CreatedAt        = new DateTime(2025, 11, 14, 8, 0, 0),
            SubmittedAt      = new DateTime(2025, 11, 14, 17, 5, 0),
            ApprovedAt       = new DateTime(2025, 11, 15, 9, 0, 0),
            PayrollRunId     = 2,
            AdminNote        = null
        },

        // Daniel – submitted
        new TimesheetEntry
        {
            TimesheetEntryId = 7,
            EmployeeId       = 1006,
            WorkDate         = new DateTime(2025, 11, 15),
            StartTime        = new TimeSpan(9, 0, 0),
            BreakStartTime   = new TimeSpan(13, 0, 0),
            BreakEndTime     = new TimeSpan(13, 30, 0),
            FinishTime       = new TimeSpan(18, 0, 0),
            TotalHours       = 8.5m,
            Status           = TimesheetStatus.Submitted,
            CreatedAt        = new DateTime(2025, 11, 15, 8, 45, 0),
            SubmittedAt      = new DateTime(2025, 11, 15, 18, 10, 0),
            ApprovedAt       = null,
            PayrollRunId     = 2,
            AdminNote        = null
        },

        // Emma – draft
        new TimesheetEntry
        {
            TimesheetEntryId = 8,
            EmployeeId       = 1007,
            WorkDate         = new DateTime(2025, 11, 16),
            StartTime        = new TimeSpan(9, 0, 0),
            BreakStartTime   = new TimeSpan(12, 30, 0),
            BreakEndTime     = new TimeSpan(13, 0, 0),
            FinishTime       = new TimeSpan(17, 30, 0),
            TotalHours       = 8.0m,
            Status           = TimesheetStatus.Draft,
            CreatedAt        = new DateTime(2025, 11, 16, 8, 40, 0),
            SubmittedAt      = null,
            ApprovedAt       = null,
            PayrollRunId     = null,
            AdminNote        = null
        },

        // Liam – approved, run 3
        new TimesheetEntry
        {
            TimesheetEntryId = 9,
            EmployeeId       = 1008,
            WorkDate         = new DateTime(2025, 11, 21),
            StartTime        = new TimeSpan(8, 0, 0),
            BreakStartTime   = new TimeSpan(12, 0, 0),
            BreakEndTime     = new TimeSpan(12, 30, 0),
            FinishTime       = new TimeSpan(16, 30, 0),
            TotalHours       = 8.0m,
            Status           = TimesheetStatus.Approved,
            CreatedAt        = new DateTime(2025, 11, 21, 7, 50, 0),
            SubmittedAt      = new DateTime(2025, 11, 21, 16, 40, 0),
            ApprovedAt       = new DateTime(2025, 11, 22, 9, 0, 0),
            PayrollRunId     = 3,
            AdminNote        = "Approved – training coverage."
        }
    );
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
        // Run 1 – past paid run
        new EmployeePayrollSummary
        {
            EmployeePayrollSummaryId = 1,
            EmployeeId      = 1001,   // Temp Admin
            PayrollPeriodId = 1,      // 2025-M11
            PayrollRunId    = 1,

            PayRate   = 50.00m,
            RateType  = RateType.Hourly,
            TotalHours= 40m,
            GrossPay  = 2000m,

            PAYE              = 200m,
            KiwiSaverEmployee = 45m,
            KiwiSaverEmployer = 45m,
            ACCLevy           = 15m,
            StudentLoan       = 0m,
            Deductions        = 260m,
            Status            = PayrollSummaryStatus.Paid,
            GeneratedAt       = new DateTime(2025, 11, 13),
            NetPay            = 1740m
        },
        new EmployeePayrollSummary
        {
            EmployeePayrollSummaryId = 2,
            EmployeeId      = 1002,   // TEMP Emp
            PayrollPeriodId = 1,
            PayrollRunId    = 1,

            PayRate   = 35.00m,
            RateType  = RateType.Hourly,
            TotalHours= 38m,
            GrossPay  = 1330m,

            PAYE              = 150m,
            KiwiSaverEmployee = 40m,
            KiwiSaverEmployer = 40m,
            ACCLevy           = 12m,
            StudentLoan       = 0m,
            Deductions        = 202m,
            Status            = PayrollSummaryStatus.Paid,
            GeneratedAt       = new DateTime(2025, 11, 13),
            NetPay            = 1128m
        },
        new EmployeePayrollSummary
        {
            EmployeePayrollSummaryId = 3,
            EmployeeId      = 1003,   // Sarah
            PayrollPeriodId = 1,
            PayrollRunId    = 1,

            PayRate   = 45.00m,
            RateType  = RateType.Hourly,
            TotalHours= 42m,
            GrossPay  = 1890m,

            PAYE              = 190m,
            KiwiSaverEmployee = 56m,
            KiwiSaverEmployer = 56m,
            ACCLevy           = 14m,
            StudentLoan       = 0m,
            Deductions        = 260m,
            Status            = PayrollSummaryStatus.Paid,
            GeneratedAt       = new DateTime(2025, 11, 13),
            NetPay            = 1630m
        },

        // Run 2 – in finalising state
        new EmployeePayrollSummary
        {
            EmployeePayrollSummaryId = 4,
            EmployeeId      = 1001,
            PayrollPeriodId = 1,
            PayrollRunId    = 2,

            PayRate   = 50.00m,
            RateType  = RateType.Hourly,
            TotalHours= 40m,
            GrossPay  = 2000m,

            PAYE              = 200m,
            KiwiSaverEmployee = 45m,
            KiwiSaverEmployer = 45m,
            ACCLevy           = 15m,
            StudentLoan       = 0m,
            Deductions        = 260m,
            Status            = PayrollSummaryStatus.Draft,
            GeneratedAt       = new DateTime(2025, 11, 20),
            NetPay            = 1740m
        },
        new EmployeePayrollSummary
        {
            EmployeePayrollSummaryId = 5,
            EmployeeId      = 1004,   // Michael
            PayrollPeriodId = 1,
            PayrollRunId    = 2,

            PayRate   = 60.00m,
            RateType  = RateType.Hourly,
            TotalHours= 37m,
            GrossPay  = 2220m,

            PAYE              = 260m,
            KiwiSaverEmployee = 67m,
            KiwiSaverEmployer = 67m,
            ACCLevy           = 17m,
            StudentLoan       = 0m,
            Deductions        = 344m,
            Status            = PayrollSummaryStatus.Draft,
            GeneratedAt       = new DateTime(2025, 11, 20),
            NetPay            = 1876m
        },

        // Run 3 – open (preview)
        new EmployeePayrollSummary
        {
            EmployeePayrollSummaryId = 6,
            EmployeeId      = 1005,
            PayrollPeriodId = 1,
            PayrollRunId    = 3,

            PayRate   = 28.00m,
            RateType  = RateType.Hourly,
            TotalHours= 30m,
            GrossPay  = 840m,

            PAYE              = 90m,
            KiwiSaverEmployee = 25m,
            KiwiSaverEmployer = 25m,
            ACCLevy           = 7m,
            StudentLoan       = 0m,
            Deductions        = 122m,
            Status            = PayrollSummaryStatus.Draft,
            GeneratedAt       = new DateTime(2025, 11, 22),
            NetPay            = 718m
        }
    );
}


private void SeedPayrollRuns(ModelBuilder b)
{
    b.Entity<PayrollRun>().HasData(
        // Past weekly run (processed/paid)
        new PayrollRun
        {
            PayrollRunId = 1,
            PeriodStart  = new DateTime(2025, 11, 6),
            PeriodEnd    = new DateTime(2025, 11, 12),
            PayFrequency = PayFrequency.Weekly,
            Status       = PayrollRunStatus.Paid,
            CreatedAt    = new DateTime(2025, 11, 13),
            ProcessedAt  = new DateTime(2025, 11, 13),
            PaidAt       = new DateTime(2025, 11, 14)
        },

        // Current weekly run – finalising
        new PayrollRun
        {
            PayrollRunId = 2,
            PeriodStart  = new DateTime(2025, 11, 13),
            PeriodEnd    = new DateTime(2025, 11, 19),
            PayFrequency = PayFrequency.Weekly,
            Status       = PayrollRunStatus.Finalizing,
            CreatedAt    = new DateTime(2025, 11, 20),
            ProcessedAt  = null,
            PaidAt       = null
        },

        // Upcoming weekly run – open
        new PayrollRun
        {
            PayrollRunId = 3,
            PeriodStart  = new DateTime(2025, 11, 20),
            PeriodEnd    = new DateTime(2025, 11, 26),
            PayFrequency = PayFrequency.Weekly,
            Status       = PayrollRunStatus.Open,
            CreatedAt    = new DateTime(2025, 11, 21),
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
            .WithMany(emp => emp.PayrollSummaries)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔹 FIX: hook to PayrollPeriod.PayrollSummaries
        e.HasOne(x => x.PayrollPeriod)
            .WithMany(p => p.PayrollSummaries)
            .HasForeignKey(x => x.PayrollPeriodId)
            .OnDelete(DeleteBehavior.Restrict);

        // Link to payroll run
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
        e.Property(x => x.NetPay).HasPrecision(14, 2);
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
            EmployeeId   = 1001,
            FirstName    = "Temp",
            LastName     = "Admin",
            Email        = "admin@nzftc.local",
            Department   = "HR",
            JobPositionId= 11,     // HR Manager
            PayGradeId   = 8,
            EmployeeCode = "NZFTC1001",
            StartDate    = new DateTime(2025, 11, 20),
            Birthday     = new DateTime(1990, 1, 1),
            Gender       = "Other",
            Address      = "N/A",
            PasswordHash = new byte[0],
            PasswordSalt = new byte[0],
            PayFrequency = PayFrequency.Weekly
        },

        // ===== SAMPLE EMPLOYEE #1 =====
        new Employee
        {
            EmployeeId   = 1002,
            FirstName    = "TEMP",
            LastName     = "Emp",
            Email        = "emp@nzftc.local",
            Department   = "Finance",
            JobPositionId= 4,     // Accountant
            PayGradeId   = 7,
            EmployeeCode = "NZFTC1002",
            StartDate    = new DateTime(2025, 11, 20),
            Birthday     = new DateTime(1995, 5, 15),
            Gender       = "Male",
            Address      = "123 Finance Street",
            PasswordHash = new byte[0],
            PasswordSalt = new byte[0],
            PayFrequency = PayFrequency.Weekly
        },

        new Employee
        {
            EmployeeId   = 1003,
            FirstName    = "Sarah",
            LastName     = "Williams",
            Email        = "sarah@nzftc.local",
            Department   = "IT",
            JobPositionId= 22,    // Software Developer
            PayGradeId   = 6,
            EmployeeCode = "NZFTC1003",
            StartDate    = new DateTime(2025, 11, 10),
            Birthday     = new DateTime(1997, 3, 12),
            Gender       = "Female",
            Address      = "42 Eden Terrace",
            PasswordHash = new byte[0],
            PasswordSalt = new byte[0],
            PayFrequency = PayFrequency.Weekly
        },

        new Employee
        {
            EmployeeId   = 1004,
            FirstName    = "Michael",
            LastName     = "Brown",
            Email        = "michael@nzftc.local",
            Department   = "Finance",
            JobPositionId= 3,     // Senior Accountant
            PayGradeId   = 8,
            EmployeeCode = "NZFTC1004",
            StartDate    = new DateTime(2025, 10, 5),
            Birthday     = new DateTime(1988, 9, 14),
            Gender       = "Male",
            Address      = "19 Queen Street",
            PasswordHash = new byte[0],
            PasswordSalt = new byte[0],
            PayFrequency = PayFrequency.Weekly
        },

        // ===== EXTRA EMPLOYEES =====

        new Employee
        {
            EmployeeId   = 1005,
            FirstName    = "Olivia",
            LastName     = "Chen",
            Email        = "olivia@nzftc.local",
            Department   = "Operations",
            JobPositionId= 32,    // Customer Service Representative
            PayGradeId   = 2,
            EmployeeCode = "NZFTC1005",
            StartDate    = new DateTime(2025, 9, 1),
            Birthday     = new DateTime(1999, 7, 22),
            Gender       = "Female",
            Address      = "8 Harbour View",
            PasswordHash = new byte[0],
            PasswordSalt = new byte[0],
            PayFrequency = PayFrequency.Weekly
        },

        new Employee
        {
            EmployeeId   = 1006,
            FirstName    = "Daniel",
            LastName     = "Lee",
            Email        = "daniel@nzftc.local",
            Department   = "IT",
            JobPositionId= 24,    // IT Support Technician
            PayGradeId   = 2,
            EmployeeCode = "NZFTC1006",
            StartDate    = new DateTime(2025, 8, 15),
            Birthday     = new DateTime(1996, 11, 3),
            Gender       = "Male",
            Address      = "55 Tech Lane",
            PasswordHash = new byte[0],
            PasswordSalt = new byte[0],
            PayFrequency = PayFrequency.Weekly
        },

        new Employee
        {
            EmployeeId   = 1007,
            FirstName    = "Emma",
            LastName     = "Johnson",
            Email        = "emma@nzftc.local",
            Department   = "Finance",
            JobPositionId= 8,     // Finance Administrator
            PayGradeId   = 2,
            EmployeeCode = "NZFTC1007",
            StartDate    = new DateTime(2025, 7, 10),
            Birthday     = new DateTime(1994, 4, 5),
            Gender       = "Female",
            Address      = "21 Ledger Street",
            PasswordHash = new byte[0],
            PasswordSalt = new byte[0],
            PayFrequency = PayFrequency.Weekly
        },

        new Employee
        {
            EmployeeId   = 1008,
            FirstName    = "Liam",
            LastName     = "Davis",
            Email        = "liam@nzftc.local",
            Department   = "Operations",
            JobPositionId= 28,    // Team Leader – Operations
            PayGradeId   = 4,
            EmployeeCode = "NZFTC1008",
            StartDate    = new DateTime(2025, 6, 1),
            Birthday     = new DateTime(1992, 2, 18),
            Gender       = "Male",
            Address      = "3 Riverbank Road",
            PasswordHash = new byte[0],
            PasswordSalt = new byte[0],
            PayFrequency = PayFrequency.Weekly
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
            EmployeeId      = 1001,
            AnnualAccrued   = 10,
            AnnualUsed      = 2,
            SickAccrued     = 5,
            SickUsed        = 1,
            CarryOverAnnual = 0,
            UpdatedAt       = new DateTime(2025, 11, 1)
        },
        new EmployeeLeaveBalance
        {
            EmployeeLeaveBalanceId = 2,
            EmployeeId      = 1002,
            AnnualAccrued   = 8,
            AnnualUsed      = 1,
            SickAccrued     = 4,
            SickUsed        = 0,
            CarryOverAnnual = 0,
            UpdatedAt       = new DateTime(2025, 11, 1)
        },
        new EmployeeLeaveBalance
        {
            EmployeeLeaveBalanceId = 3,
            EmployeeId      = 1003,
            AnnualAccrued   = 5,
            AnnualUsed      = 0,
            SickAccrued     = 2,
            SickUsed        = 0,
            CarryOverAnnual = 0,
            UpdatedAt       = new DateTime(2025, 11, 1)
        },
        new EmployeeLeaveBalance
        {
            EmployeeLeaveBalanceId = 4,
            EmployeeId      = 1004,
            AnnualAccrued   = 10,
            AnnualUsed      = 2,
            SickAccrued     = 5,
            SickUsed        = 1,
            CarryOverAnnual = 1,
            UpdatedAt       = new DateTime(2025, 11, 1)
        },
        new EmployeeLeaveBalance
        {
            EmployeeLeaveBalanceId = 5,
            EmployeeId      = 1005,
            AnnualAccrued   = 6,
            AnnualUsed      = 0,
            SickAccrued     = 3,
            SickUsed        = 0,
            CarryOverAnnual = 0,
            UpdatedAt       = new DateTime(2025, 11, 1)
        },
        new EmployeeLeaveBalance
        {
            EmployeeLeaveBalanceId = 6,
            EmployeeId      = 1006,
            AnnualAccrued   = 7,
            AnnualUsed      = 1,
            SickAccrued     = 4,
            SickUsed        = 0,
            CarryOverAnnual = 0,
            UpdatedAt       = new DateTime(2025, 11, 1)
        },
        new EmployeeLeaveBalance
        {
            EmployeeLeaveBalanceId = 7,
            EmployeeId      = 1007,
            AnnualAccrued   = 9,
            AnnualUsed      = 2,
            SickAccrued     = 5,
            SickUsed        = 1,
            CarryOverAnnual = 0,
            UpdatedAt       = new DateTime(2025, 11, 1)
        },
        new EmployeeLeaveBalance
        {
            EmployeeLeaveBalanceId = 8,
            EmployeeId      = 1008,
            AnnualAccrued   = 12,
            AnnualUsed      = 3,
            SickAccrued     = 6,
            SickUsed        = 1,
            CarryOverAnnual = 2,
            UpdatedAt       = new DateTime(2025, 11, 1)
        }
    );
}


private void SeedSupportTickets(ModelBuilder b)
{
    b.Entity<SupportTicket>().HasData(
        new SupportTicket
        {
            Id           = 1,
            Subject      = "Welcome to Support",
            Message      = "This is the initial seeded support ticket.",
            Status       = SupportStatus.Open,          // was 0
            Priority     = SupportPriority.Low,      // was 0
            EmployeeId   = 1002,                        // created by sample employee
            AssignedToId = 1001,                        // assigned to admin
            CreatedAt    = new DateTime(2025, 11, 20),
            UpdatedAt    = null
        },
        new SupportTicket
        {
            Id           = 2,
            Subject      = "Cannot submit timesheet",
            Message      = "I get an error when submitting my timesheet for this week.",
            Status       = SupportStatus.Open,
            Priority     = SupportPriority.High,        // was 1
            EmployeeId   = 1005,                        // Olivia
            AssignedToId = 1006,                        // Daniel (IT)
            CreatedAt    = new DateTime(2025, 11, 21, 9, 30, 0),
            UpdatedAt    = new DateTime(2025, 11, 21, 10, 0, 0)
        },
        new SupportTicket
        {
            Id           = 3,
            Subject      = "Payslip amount seems incorrect",
            Message      = "My overtime for last week is missing.",
            Status       = SupportStatus.InProgress,
            Priority     = SupportPriority.High,
            EmployeeId   = 1004,                        // Michael
            AssignedToId = 1001,                        // Admin
            CreatedAt    = new DateTime(2025, 11, 18, 14, 0, 0),
            UpdatedAt    = new DateTime(2025, 11, 19, 9, 0, 0)
        }
    );
}


        private void SeedSupportMessages(ModelBuilder b)
{
    b.Entity<SupportMessage>().HasData(

        // Ticket 1
        new SupportMessage
        {
            Id                = 1,
            TicketId          = 1,
            SenderEmployeeId  = 1002,
            SenderIsAdmin     = false,
            Body              = "Hi, I need help setting up my account.",
            SentAt            = new DateTime(2025, 11, 20, 9, 0, 0)
        },
        new SupportMessage
        {
            Id                = 2,
            TicketId          = 1,
            SenderEmployeeId  = 1001,
            SenderIsAdmin     = true,
            Body              = "Admin here — your account is now active!",
            SentAt            = new DateTime(2025, 11, 20, 9, 5, 0)
        },

        // Ticket 2
        new SupportMessage
        {
            Id                = 3,
            TicketId          = 2,
            SenderEmployeeId  = 1005,
            SenderIsAdmin     = false,
            Body              = "I get a red error banner when I click submit.",
            SentAt            = new DateTime(2025, 11, 21, 9, 32, 0)
        },
        new SupportMessage
        {
            Id                = 4,
            TicketId          = 2,
            SenderEmployeeId  = 1006,
            SenderIsAdmin     = true,
            Body              = "Can you send me a screenshot of the error?",
            SentAt            = new DateTime(2025, 11, 21, 9, 45, 0)
        },

        // Ticket 3
        new SupportMessage
        {
            Id                = 5,
            TicketId          = 3,
            SenderEmployeeId  = 1004,
            SenderIsAdmin     = false,
            Body              = "My timesheet shows 42 hours but payslip only paid 40.",
            SentAt            = new DateTime(2025, 11, 18, 14, 5, 0)
        },
        new SupportMessage
        {
            Id                = 6,
            TicketId          = 3,
            SenderEmployeeId  = 1001,
            SenderIsAdmin     = true,
            Body              = "We’re checking your timesheet against the payroll run.",
            SentAt            = new DateTime(2025, 11, 19, 9, 10, 0)
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
            Id          = 1,
            Title       = "System Go-Live",
            Description = "NZFTC EMS officially launched!",
            Start       = new DateTime(2025, 11, 20, 9, 0, 0),
            End         = new DateTime(2025, 11, 20, 17, 0, 0),
            EventType   = CalendarEventType.Other,
            OwnerUsername = "System",
            IsTodo      = false,
            IsPublicHoliday = false
        },
        // Stand-up – IT
        new CalendarEvent
        {
            Id          = 2,
            Title       = "Daily Stand-up – IT",
            Description = "15-minute catch-up for IT team.",
            Start       = new DateTime(2025, 11, 21, 9, 0, 0),
            End         = new DateTime(2025, 11, 21, 9, 15, 0),
            EventType   = CalendarEventType.Meeting,
            OwnerUsername = "sarah@nzftc.local",
            IsTodo      = false,
            IsPublicHoliday = false
        },
        // Payroll cut-off reminder
        new CalendarEvent
        {
            Id          = 3,
            Title       = "Payroll Cut-off",
            Description = "Approve timesheets before 3 PM.",
            Start       = new DateTime(2025, 11, 19, 15, 0, 0),
            End         = new DateTime(2025, 11, 19, 15, 30, 0),
            EventType   = CalendarEventType.Other,
            OwnerUsername = "admin@nzftc.local",
            IsTodo      = true,
            IsPublicHoliday = false
        },
        // Approved sick leave – Sarah
        new CalendarEvent
        {
            Id          = 4,
            Title       = "Approved leave – Sarah Williams",
            Description = "Sick leave",
            Start       = new DateTime(2025, 11, 18, 0, 0, 0),
            End         = new DateTime(2025, 11, 20, 0, 0, 0), // end exclusive
            EventType   = CalendarEventType.Leave,
            OwnerUsername = "sarah@nzftc.local",
            IsTodo      = false,
            IsPublicHoliday = false
        },
        // Approved annual – Daniel
        new CalendarEvent
        {
            Id          = 5,
            Title       = "Approved leave – Daniel Lee",
            Description = "Annual leave",
            Start       = new DateTime(2025, 12, 10, 0, 0, 0),
            End         = new DateTime(2025, 12, 12, 0, 0, 0),
            EventType   = CalendarEventType.Leave,
            OwnerUsername = "daniel@nzftc.local",
            IsTodo      = false,
            IsPublicHoliday = false
        }
    );
}
  }
}
