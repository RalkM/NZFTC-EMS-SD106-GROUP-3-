using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZFTC_EMS.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Timesheets_Leave_Grievances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Body = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "calendarevents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    OwnerUsername = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsTodo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPublicHoliday = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calendarevents", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "holidays",
                columns: table => new
                {
                    HolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HolidayDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    HolidayType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPaidHoliday = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_holidays", x => x.HolidayId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "leavepolicies",
                columns: table => new
                {
                    LeavePolicyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AnnualDefault = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AnnualAccrualRate = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AnnualCarryOverLimit = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AllowNegativeAnnual = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SickDefault = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SickAccrualRate = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AllowUnpaidSick = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CustomLeaveTypesJson = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leavepolicies", x => x.LeavePolicyId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "paygrades",
                columns: table => new
                {
                    PayGradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BaseRate = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RateType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paygrades", x => x.PayGradeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payrollperiods",
                columns: table => new
                {
                    PayrollPeriodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PeriodCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PeriodStart = table.Column<DateTime>(type: "date", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "date", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false),
                    Closed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrollperiods", x => x.PayrollPeriodId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payrollruns",
                columns: table => new
                {
                    PayrollRunId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PeriodStart = table.Column<DateTime>(type: "date", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "date", nullable: false),
                    PayFrequency = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrollruns", x => x.PayrollRunId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payrollsettings",
                columns: table => new
                {
                    PayrollSettingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KiwiSaverEmployeePercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    KiwiSaverEmployerPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ACCLevyPercent = table.Column<decimal>(type: "decimal(5,3)", precision: 5, scale: 3, nullable: false),
                    EnableStudentLoan = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RegularHoursPerWeek = table.Column<int>(type: "int", nullable: false),
                    OvertimeMultiplier = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrollsettings", x => x.PayrollSettingsId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "jobpositions",
                columns: table => new
                {
                    JobPositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Department = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PayGradeId = table.Column<int>(type: "int", nullable: false),
                    AccessRole = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobpositions", x => x.JobPositionId);
                    table.ForeignKey(
                        name: "FK_jobpositions_paygrades_PayGradeId",
                        column: x => x.PayGradeId,
                        principalTable: "paygrades",
                        principalColumn: "PayGradeId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Gender = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<byte[]>(type: "longblob", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "longblob", nullable: true),
                    Department = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PayFrequency = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    JobPositionId = table.Column<int>(type: "int", nullable: true),
                    PayGradeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_jobpositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalTable: "jobpositions",
                        principalColumn: "JobPositionId");
                    table.ForeignKey(
                        name: "FK_Employees_paygrades_PayGradeId",
                        column: x => x.PayGradeId,
                        principalTable: "paygrades",
                        principalColumn: "PayGradeId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employeeemergencycontacts",
                columns: table => new
                {
                    EmergencyContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Relationship = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeemergencycontacts", x => x.EmergencyContactId);
                    table.ForeignKey(
                        name: "FK_employeeemergencycontacts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employeeleavebalances",
                columns: table => new
                {
                    EmployeeLeaveBalanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AnnualAccrued = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AnnualUsed = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SickAccrued = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SickUsed = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CarryOverAnnual = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeleavebalances", x => x.EmployeeLeaveBalanceId);
                    table.ForeignKey(
                        name: "FK_employeeleavebalances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmployeePayrollSummaries",
                columns: table => new
                {
                    EmployeePayrollSummaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PayrollPeriodId = table.Column<int>(type: "int", nullable: true),
                    PayrollRunId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PayRate = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    RateType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    GrossPay = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PAYE = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    KiwiSaverEmployee = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    KiwiSaverEmployer = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ACCLevy = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    StudentLoan = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Deductions = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalHours = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NetPay = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayrollSummaries", x => x.EmployeePayrollSummaryId);
                    table.ForeignKey(
                        name: "FK_EmployeePayrollSummaries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeePayrollSummaries_payrollperiods_PayrollPeriodId",
                        column: x => x.PayrollPeriodId,
                        principalTable: "payrollperiods",
                        principalColumn: "PayrollPeriodId");
                    table.ForeignKey(
                        name: "FK_EmployeePayrollSummaries_payrollruns_PayrollRunId",
                        column: x => x.PayrollRunId,
                        principalTable: "payrollruns",
                        principalColumn: "PayrollRunId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Grievances",
                columns: table => new
                {
                    GrievanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubmittedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EmployeeMessage = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdminResponse = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grievances", x => x.GrievanceId);
                    table.ForeignKey(
                        name: "FK_Grievances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "leaverequests",
                columns: table => new
                {
                    LeaveRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LeaveType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    Reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequestedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ApprovedByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leaverequests", x => x.LeaveRequestId);
                    table.ForeignKey(
                        name: "FK_leaverequests_Employees_ApprovedByEmployeeId",
                        column: x => x.ApprovedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_leaverequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "supporttickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Message = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    AssignedToId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supporttickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_supporttickets_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "timesheetentries",
                columns: table => new
                {
                    TimesheetEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    WorkDate = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    BreakStartTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    BreakEndTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    FinishTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    TotalHours = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PayrollRunId = table.Column<int>(type: "int", nullable: true),
                    AdminNote = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timesheetentries", x => x.TimesheetEntryId);
                    table.ForeignKey(
                        name: "FK_timesheetentries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_timesheetentries_payrollruns_PayrollRunId",
                        column: x => x.PayrollRunId,
                        principalTable: "payrollruns",
                        principalColumn: "PayrollRunId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "supportmessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    SenderEmployeeId = table.Column<int>(type: "int", nullable: true),
                    Body = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SentAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SenderIsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AdminReply = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdminReplyAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supportmessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_supportmessages_supporttickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "supporttickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "calendarevents",
                columns: new[] { "Id", "Description", "End", "EventType", "IsPublicHoliday", "IsTodo", "OwnerUsername", "Start", "Title" },
                values: new object[] { 1, "NZFTC EMS officially launched!", new DateTime(2025, 11, 20, 17, 0, 0, 0, DateTimeKind.Unspecified), 3, false, false, "System", new DateTime(2025, 11, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "System Go-Live" });

            migrationBuilder.InsertData(
                table: "holidays",
                columns: new[] { "HolidayId", "HolidayDate", "HolidayType", "IsPaidHoliday", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "New Year's Day" },
                    { 2, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Day After New Year's Day" },
                    { 3, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Waitangi Day" },
                    { 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Good Friday" },
                    { 5, new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Easter Monday" },
                    { 6, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "ANZAC Day" },
                    { 7, new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "King's Birthday" },
                    { 8, new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Matariki" },
                    { 9, new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Labour Day" },
                    { 10, new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Christmas Day" },
                    { 11, new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Boxing Day" }
                });

            migrationBuilder.InsertData(
                table: "leavepolicies",
                columns: new[] { "LeavePolicyId", "AllowNegativeAnnual", "AllowUnpaidSick", "AnnualAccrualRate", "AnnualCarryOverLimit", "AnnualDefault", "CustomLeaveTypesJson", "SickAccrualRate", "SickDefault", "UpdatedAt" },
                values: new object[] { 1, false, true, 1.67m, 5m, 20m, "[]", 0m, 10m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "paygrades",
                columns: new[] { "PayGradeId", "BaseRate", "Description", "IsActive", "Name", "RateType" },
                values: new object[,]
                {
                    { 1, 23.50m, "Minimum Pay", true, "PG-minimum", (byte)0 },
                    { 2, 28.00m, "Regular Staff", true, "PG-REG", (byte)0 },
                    { 3, 32.00m, "Senior Staff", true, "PG-SSTAFF", (byte)0 },
                    { 4, 38.00m, "Team Leader", true, "PG-TL", (byte)0 },
                    { 5, 45.00m, "Supervisor", true, "PG-SUP", (byte)0 },
                    { 6, 50.00m, "Specialist", true, "PG-SPEC", (byte)0 },
                    { 7, 65000m, "Salary - Junior/Assistant Manager", true, "PG-SAL-JM", (byte)1 },
                    { 8, 80000m, "Salary - Department Manager", true, "PG-SAL-MAN", (byte)1 },
                    { 9, 95000m, "Salary - Senior Manager", true, "PG-SAL-SRMAN", (byte)1 },
                    { 10, 100000m, "Salary - General Manager", true, "PG-SAL-GM", (byte)1 }
                });

            migrationBuilder.InsertData(
                table: "payrollperiods",
                columns: new[] { "PayrollPeriodId", "Closed", "PeriodCode", "PeriodEnd", "PeriodStart", "TotalAmount" },
                values: new object[,]
                {
                    { 1, false, "2025-M11", new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m },
                    { 2, false, "2025-M12", new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m },
                    { 3, true, "2025-FN13", new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90000m }
                });

            migrationBuilder.InsertData(
                table: "payrollruns",
                columns: new[] { "PayrollRunId", "CreatedAt", "PaidAt", "PayFrequency", "PeriodEnd", "PeriodStart", "ProcessedAt", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 2, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 }
                });

            migrationBuilder.InsertData(
                table: "payrollsettings",
                columns: new[] { "PayrollSettingsId", "ACCLevyPercent", "EnableStudentLoan", "KiwiSaverEmployeePercent", "KiwiSaverEmployerPercent", "OvertimeMultiplier", "RegularHoursPerWeek", "UpdatedAt" },
                values: new object[] { 1, 1.53m, true, 3.0m, 3.0m, 1.5m, 40, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "jobpositions",
                columns: new[] { "JobPositionId", "AccessRole", "Department", "Description", "IsActive", "Name", "PayGradeId" },
                values: new object[,]
                {
                    { 1, "Admin", "Finance", null, true, "Chief Financial Officer (CFO)", 10 },
                    { 2, "Admin", "Finance", null, true, "Finance Manager", 9 },
                    { 3, "Employee", "Finance", null, true, "Senior Accountant", 8 },
                    { 4, "Employee", "Finance", null, true, "Accountant", 7 },
                    { 5, "Employee", "Finance", null, true, "Accounts Payable Officer", 3 },
                    { 6, "Employee", "Finance", null, true, "Accounts Receivable Officer", 3 },
                    { 7, "Employee", "Finance", null, true, "Payroll Officer", 6 },
                    { 8, "Employee", "Finance", null, true, "Finance Administrator", 2 },
                    { 9, "Employee", "Finance", null, true, "Billing Specialist", 2 },
                    { 10, "Employee", "Finance", null, true, "Accounts Assistant", 1 },
                    { 11, "Admin", "HR", null, true, "HR Manager", 8 },
                    { 12, "Admin", "HR", null, true, "Senior HR Advisor", 7 },
                    { 13, "Admin", "HR", null, true, "HR Advisor", 3 },
                    { 14, "Admin", "HR", null, true, "HR Coordinator", 2 },
                    { 15, "Admin", "HR", null, true, "HR Administrator", 1 },
                    { 16, "Admin", "HR", null, true, "Recruitment Specialist", 6 },
                    { 17, "Admin", "HR", null, true, "Talent Acquisition Coordinator", 3 },
                    { 18, "Admin", "HR", null, true, "Training & Development Officer", 6 },
                    { 19, "Admin", "IT", null, true, "IT Manager", 8 },
                    { 20, "Employee", "IT", null, true, "Systems Administrator", 3 },
                    { 21, "Employee", "IT", null, true, "Network Administrator", 6 },
                    { 22, "Employee", "IT", null, true, "Software Developer", 6 },
                    { 23, "Employee", "IT", null, true, "Application Support Analyst", 3 },
                    { 24, "Employee", "IT", null, true, "IT Support Technician", 2 },
                    { 25, "Employee", "IT", null, true, "Helpdesk Support", 1 },
                    { 26, "Employee", "IT", null, true, "Database Administrator (DBA)", 6 },
                    { 27, "Admin", "Operations", null, true, "Operations Manager", 8 },
                    { 28, "Employee", "Operations", null, true, "Team Leader – Operations", 4 },
                    { 29, "Employee", "Operations", null, true, "Supervisor – Operations", 5 },
                    { 30, "Employee", "Operations", null, true, "Senior Officer – Operations", 3 },
                    { 31, "Employee", "Operations", null, true, "Office Administrator", 2 },
                    { 32, "Employee", "Operations", null, true, "Customer Service Representative", 2 },
                    { 33, "Employee", "Operations", null, true, "Data Entry Operator", 1 }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Address", "Birthday", "Department", "Email", "EmployeeCode", "FirstName", "Gender", "JobPositionId", "LastName", "PasswordHash", "PasswordSalt", "PayFrequency", "PayGradeId", "Phone", "StartDate" },
                values: new object[,]
                {
                    { 1001, "N/A", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", "admin@nzftc.local", "NZFTC1001", "Temp", "Other", 11, "Admin", new byte[0], new byte[0], (byte)0, 8, null, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1002, "123 Finance Street", new DateTime(1995, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", "emp@nzftc.local", "NZFTC1002", "TEMP", "Male", 4, "Emp", new byte[0], new byte[0], (byte)0, 7, null, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1003, "42 Eden Terrace", new DateTime(1997, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "IT", "sarah@nzftc.local", "NZFTC1003", "Sarah", "Female", 22, "Williams", new byte[0], new byte[0], (byte)0, 6, null, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1004, "19 Queen Street", new DateTime(1988, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", "michael@nzftc.local", "NZFTC1004", "Michael", "Male", 3, "Brown", new byte[0], new byte[0], (byte)0, 8, null, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "EmployeePayrollSummaries",
                columns: new[] { "EmployeePayrollSummaryId", "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PaidAt", "PayRate", "PayrollPeriodId", "PayrollRunId", "RateType", "Status", "StudentLoan", "TotalHours" },
                values: new object[,]
                {
                    { 1, 15m, 260m, 1001, new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, null, 50.00m, 1, 1, (byte)0, (byte)2, 0m, 40m },
                    { 2, 15m, 260m, 1001, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, null, 50.00m, 1, 2, (byte)0, (byte)2, 0m, 40m }
                });

            migrationBuilder.InsertData(
                table: "Grievances",
                columns: new[] { "GrievanceId", "AdminResponse", "EmployeeId", "EmployeeMessage", "Status", "Subject", "SubmittedAt" },
                values: new object[,]
                {
                    { 1, null, 1002, "My roster has been changed without notice and clashes with study.", (byte)0, "Roster concerns", new DateTime(2025, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "IT has been notified and will replace your workstation this week.", 1003, "My workstation keeps freezing and affects my productivity.", (byte)1, "Equipment not working", new DateTime(2025, 11, 18, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "We have recalculated and processed an adjustment in your next pay.", 1004, "I believe my overtime for October was underpaid.", (byte)3, "Payroll discrepancy – October", new DateTime(2025, 11, 10, 14, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "employeeemergencycontacts",
                columns: new[] { "EmergencyContactId", "Email", "EmployeeId", "FullName", "Phone", "Relationship" },
                values: new object[,]
                {
                    { 1, "none@local", 1001, "Admin Contact", "0000", null },
                    { 2, "jane.doe@example.com", 1002, "Jane Doe", "0211234567", null }
                });

            migrationBuilder.InsertData(
                table: "employeeleavebalances",
                columns: new[] { "EmployeeLeaveBalanceId", "AnnualAccrued", "AnnualUsed", "CarryOverAnnual", "EmployeeId", "SickAccrued", "SickUsed", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 0m, 0m, 0m, 1001, 0m, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 0m, 0m, 0m, 1002, 0m, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 5m, 0m, 0m, 1003, 2m, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 10m, 2m, 0m, 1004, 5m, 1m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "leaverequests",
                columns: new[] { "LeaveRequestId", "ApprovedAt", "ApprovedByEmployeeId", "EmployeeId", "EndDate", "LeaveType", "Reason", "RequestedAt", "StartDate" },
                values: new object[] { 1, null, null, 1002, new DateTime(2025, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual", "Family event", new DateTime(2025, 11, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "leaverequests",
                columns: new[] { "LeaveRequestId", "ApprovedAt", "ApprovedByEmployeeId", "EmployeeId", "EndDate", "LeaveType", "Reason", "RequestedAt", "StartDate", "Status" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 11, 17, 14, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1003, new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sick", "Flu", new DateTime(2025, 11, 17, 9, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" },
                    { 3, new DateTime(2025, 11, 23, 16, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1004, new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual", "Overlaps with year-end close", new DateTime(2025, 11, 22, 11, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "supporttickets",
                columns: new[] { "Id", "AssignedToId", "CreatedAt", "EmployeeId", "Message", "Priority", "Status", "Subject", "UpdatedAt" },
                values: new object[] { 1, 1001, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1002, "This is the initial seeded support ticket.", 0, 0, "Welcome to Support", null });

            migrationBuilder.InsertData(
                table: "timesheetentries",
                columns: new[] { "TimesheetEntryId", "AdminNote", "ApprovedAt", "BreakEndTime", "BreakStartTime", "CreatedAt", "EmployeeId", "FinishTime", "PayrollRunId", "StartTime", "Status", "SubmittedAt", "TotalHours", "WorkDate" },
                values: new object[,]
                {
                    { 1, "Approved – standard day.", new DateTime(2025, 11, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 30, 0, 0), new TimeSpan(0, 12, 0, 0, 0), new DateTime(2025, 11, 7, 8, 30, 0, 0, DateTimeKind.Unspecified), 1001, new TimeSpan(0, 17, 0, 0, 0), 1, new TimeSpan(0, 9, 0, 0, 0), 2, new DateTime(2025, 11, 7, 17, 5, 0, 0, DateTimeKind.Unspecified), 7.5m, new DateTime(2025, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, null, null, new TimeSpan(0, 13, 0, 0, 0), new TimeSpan(0, 12, 30, 0, 0), new DateTime(2025, 11, 11, 8, 0, 0, 0, DateTimeKind.Unspecified), 1002, new TimeSpan(0, 17, 0, 0, 0), null, new TimeSpan(0, 8, 30, 0, 0), 1, new DateTime(2025, 11, 11, 17, 10, 0, 0, DateTimeKind.Unspecified), 8.0m, new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, null, null, new TimeSpan(0, 13, 0, 0, 0), new TimeSpan(0, 12, 30, 0, 0), new DateTime(2025, 11, 12, 8, 45, 0, 0, DateTimeKind.Unspecified), 1002, new TimeSpan(0, 18, 0, 0, 0), null, new TimeSpan(0, 9, 0, 0, 0), 0, null, 8.5m, new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Approved – includes 0.5h overtime.", new DateTime(2025, 11, 11, 9, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 30, 0, 0), new TimeSpan(0, 13, 0, 0, 0), new DateTime(2025, 11, 10, 8, 40, 0, 0, DateTimeKind.Unspecified), 1003, new TimeSpan(0, 18, 0, 0, 0), 1, new TimeSpan(0, 9, 0, 0, 0), 2, new DateTime(2025, 11, 10, 18, 5, 0, 0, DateTimeKind.Unspecified), 8.5m, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Rejected – incorrect break time; please resubmit.", new DateTime(2025, 11, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 14, 30, 0, 0), new TimeSpan(0, 14, 0, 0, 0), new DateTime(2025, 11, 9, 9, 30, 0, 0, DateTimeKind.Unspecified), 1004, new TimeSpan(0, 19, 0, 0, 0), null, new TimeSpan(0, 10, 0, 0, 0), 3, new DateTime(2025, 11, 9, 19, 10, 0, 0, DateTimeKind.Unspecified), 8.5m, new DateTime(2025, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "supportmessages",
                columns: new[] { "Id", "AdminReply", "AdminReplyAt", "Body", "SenderEmployeeId", "SenderIsAdmin", "SentAt", "TicketId" },
                values: new object[,]
                {
                    { 1, null, null, "Hi, I need help setting up my account.", 1002, false, new DateTime(2025, 11, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, null, null, "Admin here — your account is now active!", 1001, true, new DateTime(2025, 11, 20, 9, 5, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_employeeemergencycontacts_EmployeeId",
                table: "employeeemergencycontacts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_employeeleavebalances_EmployeeId",
                table: "employeeleavebalances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayrollSummaries_EmployeeId",
                table: "EmployeePayrollSummaries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayrollSummaries_PayrollPeriodId",
                table: "EmployeePayrollSummaries",
                column: "PayrollPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayrollSummaries_PayrollRunId",
                table: "EmployeePayrollSummaries",
                column: "PayrollRunId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobPositionId",
                table: "Employees",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PayGradeId",
                table: "Employees",
                column: "PayGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Grievances_EmployeeId",
                table: "Grievances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_holidays_HolidayDate",
                table: "holidays",
                column: "HolidayDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_jobpositions_Name",
                table: "jobpositions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_jobpositions_PayGradeId",
                table: "jobpositions",
                column: "PayGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_leaverequests_ApprovedByEmployeeId",
                table: "leaverequests",
                column: "ApprovedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_leaverequests_EmployeeId",
                table: "leaverequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_leaverequests_StartDate_EndDate",
                table: "leaverequests",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_paygrades_Name",
                table: "paygrades",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payrollperiods_PeriodCode",
                table: "payrollperiods",
                column: "PeriodCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_supportmessages_TicketId",
                table: "supportmessages",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_supporttickets_EmployeeId",
                table: "supporttickets",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_timesheetentries_EmployeeId",
                table: "timesheetentries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_timesheetentries_PayrollRunId",
                table: "timesheetentries",
                column: "PayrollRunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "calendarevents");

            migrationBuilder.DropTable(
                name: "employeeemergencycontacts");

            migrationBuilder.DropTable(
                name: "employeeleavebalances");

            migrationBuilder.DropTable(
                name: "EmployeePayrollSummaries");

            migrationBuilder.DropTable(
                name: "Grievances");

            migrationBuilder.DropTable(
                name: "holidays");

            migrationBuilder.DropTable(
                name: "leavepolicies");

            migrationBuilder.DropTable(
                name: "leaverequests");

            migrationBuilder.DropTable(
                name: "payrollsettings");

            migrationBuilder.DropTable(
                name: "supportmessages");

            migrationBuilder.DropTable(
                name: "timesheetentries");

            migrationBuilder.DropTable(
                name: "payrollperiods");

            migrationBuilder.DropTable(
                name: "supporttickets");

            migrationBuilder.DropTable(
                name: "payrollruns");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "jobpositions");

            migrationBuilder.DropTable(
                name: "paygrades");
        }
    }
}
