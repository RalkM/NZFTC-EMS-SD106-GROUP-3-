using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZFTC_EMS.Migrations
{
    /// <inheritdoc />
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
>>>>>>> Stashed changes
=======
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
>>>>>>> Stashed changes
=======
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
>>>>>>> Stashed changes
    public partial class InitialBuild : Migration
========
    public partial class Announcement : Migration
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
========
    public partial class Announcement : Migration
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
>>>>>>> Stashed changes
=======
=======
>>>>>>> Stashed changes
========
    public partial class Announcement : Migration
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
========
    public partial class Announcement : Migration
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
========
    public partial class Announcement : Migration
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
>>>>>>> Stashed changes
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                name: "CalendarEvents",
========
=======
=======
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
>>>>>>> Stashed changes
=======
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
>>>>>>> Stashed changes
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
                name: "CalendarEvents",
========
========
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
========
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
>>>>>>> Stashed changes
=======
========
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
========
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
>>>>>>> Stashed changes
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
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
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
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEvents", x => x.Id);
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
                    HolidayDate = table.Column<DateTime>(type: "date", nullable: false),
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
                    TotalAmount = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: true),
                    Closed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrollperiods", x => x.PayrollPeriodId);
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
                name: "employees",
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
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    JobPositionId = table.Column<int>(type: "int", nullable: true),
                    PayGradeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_employees_jobpositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalTable: "jobpositions",
                        principalColumn: "JobPositionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_employees_paygrades_PayGradeId",
                        column: x => x.PayGradeId,
                        principalTable: "paygrades",
                        principalColumn: "PayGradeId",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_employeeemergencycontacts_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
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
                    AnnualAccrued = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    AnnualUsed = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    SickAccrued = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    SickUsed = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CarryOverAnnual = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeleavebalances", x => x.EmployeeLeaveBalanceId);
                    table.ForeignKey(
                        name: "FK_employeeleavebalances_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employeepayrollsummaries",
                columns: table => new
                {
                    EmployeePayrollSummaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PayrollPeriodId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PayRate = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    GrossPay = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false),
                    Deductions = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false),
                    NetPay = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false, computedColumnSql: "(`GrossPay` - `Deductions`)", stored: true),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeepayrollsummaries", x => x.EmployeePayrollSummaryId);
                    table.ForeignKey(
                        name: "FK_employeepayrollsummaries_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_employeepayrollsummaries_payrollperiods_PayrollPeriodId",
                        column: x => x.PayrollPeriodId,
                        principalTable: "payrollperiods",
                        principalColumn: "PayrollPeriodId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "grievances",
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
                    table.PrimaryKey("PK_grievances", x => x.GrievanceId);
                    table.ForeignKey(
                        name: "FK_grievances_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
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
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ApprovedByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    ApprovedByEmployeeEmployeeId = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leaverequests", x => x.LeaveRequestId);
                    table.ForeignKey(
                        name: "FK_leaverequests_employees_ApprovedByEmployeeEmployeeId",
                        column: x => x.ApprovedByEmployeeEmployeeId,
                        principalTable: "employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_leaverequests_employees_ApprovedByEmployeeId",
                        column: x => x.ApprovedByEmployeeId,
                        principalTable: "employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_leaverequests_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
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
                        name: "FK_supporttickets_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "EmployeeId");
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
                table: "CalendarEvents",
                columns: new[] { "Id", "Description", "End", "EventType", "OwnerUsername", "Start", "Title" },
                values: new object[] { 1, "NZFTC EMS officially launched!", new DateTime(2025, 11, 20, 17, 0, 0, 0, DateTimeKind.Unspecified), 3, "System", new DateTime(2025, 11, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "System Go-Live" });

            migrationBuilder.InsertData(
                table: "holidays",
                columns: new[] { "HolidayId", "HolidayDate", "HolidayType", "IsPaidHoliday", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "New Year's Day" },
                    { 2, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Waitangi Day" },
                    { 3, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Good Friday" },
                    { 4, new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "Easter Monday" },
                    { 5, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public", true, "ANZAC Day" }
                });

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
                    { 7, 65000.00m, "Salary - Junior/Assistant Manager", true, "PG-SAL-JM", (byte)1 },
                    { 8, 80000.00m, "Salary - Department Manager", true, "PG-SAL-MAN", (byte)1 },
                    { 9, 95000.00m, "Salary - Senior Manager", true, "PG-SAL-SRMAN", (byte)1 },
                    { 10, 100000.00m, "Salary - General Manager", true, "PG-SAL-GM", (byte)1 }
                });

            migrationBuilder.InsertData(
                table: "payrollperiods",
                columns: new[] { "PayrollPeriodId", "Closed", "PeriodCode", "PeriodEnd", "PeriodStart", "TotalAmount" },
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
                values: new object[] { 1, false, "2025-11-M1", new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.00m });
========
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
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs

            migrationBuilder.InsertData(
                table: "jobpositions",
                columns: new[] { "JobPositionId", "AccessRole", "Department", "Description", "IsActive", "Name", "PayGradeId" },
                values: new object[,]
                {
                    { 1, "Admin", "Finance", "Top-level finance leadership", true, "Chief Financial Officer (CFO)", 10 },
                    { 2, "Admin", "Finance", "Leads the finance team and reporting", true, "Finance Manager", 9 },
                    { 3, "Employee", "Finance", "Handles complex accounting and reporting", true, "Senior Accountant", 8 },
                    { 4, "Employee", "Finance", "General accounting duties", true, "Accountant", 7 },
                    { 5, "Employee", "Finance", "Manages supplier invoices and payments", true, "Accounts Payable Officer", 3 },
                    { 6, "Employee", "Finance", "Manages customer invoicing and collections", true, "Accounts Receivable Officer", 3 },
                    { 7, "Employee", "Finance", "Processes staff payroll", true, "Payroll Officer", 6 },
                    { 8, "Employee", "Finance", "Provides general admin support to finance", true, "Finance Administrator", 2 },
                    { 9, "Employee", "Finance", "Prepares and manages billing", true, "Billing Specialist", 2 },
                    { 10, "Employee", "Finance", "Entry-level support in finance", true, "Accounts Assistant", 1 },
                    { 11, "Admin", "HR", "Leads HR operations and strategy", true, "HR Manager", 8 },
                    { 12, "Admin", "HR", "Senior advisory role in HR", true, "Senior HR Advisor", 7 },
                    { 13, "Admin", "HR", "Generalist HR support", true, "HR Advisor", 3 },
                    { 14, "Admin", "HR", "Coordinates HR processes and documentation", true, "HR Coordinator", 2 },
                    { 15, "Admin", "HR", "Admin support across HR functions", true, "HR Administrator", 1 },
                    { 16, "Admin", "HR", "Manages recruitment and selection", true, "Recruitment Specialist", 6 },
                    { 17, "Admin", "HR", "Supports talent acquisition activities", true, "Talent Acquisition Coordinator", 3 },
                    { 18, "Admin", "HR", "Coordinates training and staff development", true, "Training & Development Officer", 6 },
                    { 19, "Admin", "IT", "Leads IT operations and projects", true, "IT Manager", 8 },
                    { 20, "Employee", "IT", "Maintains servers and systems", true, "Systems Administrator", 3 },
                    { 21, "Employee", "IT", "Manages network infrastructure", true, "Network Administrator", 6 },
                    { 22, "Employee", "IT", "Develops and maintains software applications", true, "Software Developer", 6 },
                    { 23, "Employee", "IT", "Supports business applications", true, "Application Support Analyst", 3 },
                    { 24, "Employee", "IT", "First-line IT support", true, "IT Support Technician", 2 },
                    { 25, "Employee", "IT", "Handles basic IT helpdesk requests", true, "Helpdesk Support", 1 },
                    { 26, "Employee", "IT", "Manages databases and performance", true, "Database Administrator (DBA)", 6 },
                    { 27, "Admin", "Operations", "Oversees day-to-day operations", true, "Operations Manager", 8 },
                    { 28, "Employee", "Operations", "Leads an operations team", true, "Team Leader – Operations", 4 },
                    { 29, "Employee", "Operations", "Supervises operational staff", true, "Supervisor – Operations", 5 },
                    { 30, "Employee", "Operations", "Senior operations officer role", true, "Senior Officer – Operations", 3 },
                    { 31, "Employee", "Operations", "General office administration", true, "Office Administrator", 2 },
                    { 32, "Employee", "Operations", "Frontline customer service", true, "Customer Service Representative", 2 },
                    { 33, "Employee", "Operations", "Data entry and basic admin tasks", true, "Data Entry Operator", 1 }
                });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "EmployeeId", "Address", "Birthday", "Department", "Email", "EmployeeCode", "FirstName", "Gender", "JobPositionId", "LastName", "PasswordHash", "PasswordSalt", "PayGradeId", "Phone", "StartDate" },
                values: new object[,]
                {
                    { 1001, "N/A", null, "HR", "admin@nzftc.local", "TEMP001", "Temp", "N/A", 11, "Admin", new byte[0], new byte[0], 8, null, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1002, "N/A", null, "Operations", "emp@nzftc.local", "TEMP002", "Temp", "N/A", 31, "Employee", new byte[0], new byte[0], 2, null, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
                table: "EmployeePayrollSummaries",
                columns: new[] { "EmployeePayrollSummaryId", "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PaidAt", "PayRate", "PayrollPeriodId", "PayrollRunId", "RateType", "Status", "StudentLoan", "TotalHours" },
                values: new object[,]
                {
                    { 1, 15m, 260m, 1001, new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, null, 50.00m, 1, 1, (byte)0, (byte)2, 0m, 40m },
                    { 2, 15m, 260m, 1001, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, null, 50.00m, 1, 2, (byte)0, (byte)2, 0m, 40m }
                });

            migrationBuilder.InsertData(
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
=======
>>>>>>> Stashed changes
                table: "EmployeePayrollSummaries",
                columns: new[] { "EmployeePayrollSummaryId", "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PaidAt", "PayRate", "PayrollPeriodId", "PayrollRunId", "RateType", "Status", "StudentLoan", "TotalHours" },
                values: new object[,]
                {
                    { 1, 15m, 260m, 1001, new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, null, 50.00m, 1, 1, (byte)0, (byte)2, 0m, 40m },
                    { 2, 15m, 260m, 1001, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, null, 50.00m, 1, 2, (byte)0, (byte)2, 0m, 40m }
                });

            migrationBuilder.InsertData(
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
                table: "EmployeePayrollSummaries",
                columns: new[] { "EmployeePayrollSummaryId", "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PaidAt", "PayRate", "PayrollPeriodId", "PayrollRunId", "RateType", "Status", "StudentLoan", "TotalHours" },
                values: new object[,]
                {
                    { 1, 15m, 260m, 1001, new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, null, 50.00m, 1, 1, (byte)0, (byte)2, 0m, 40m },
                    { 2, 15m, 260m, 1001, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, null, 50.00m, 1, 2, (byte)0, (byte)2, 0m, 40m }
                });

            migrationBuilder.InsertData(
>>>>>>> Stashed changes
                table: "employeeemergencycontacts",
                columns: new[] { "EmergencyContactId", "Email", "EmployeeId", "FullName", "Phone", "Relationship" },
                values: new object[,]
                {
                    { 1, "none@local", 1001, "Temp Admin Contact", "0000", "N/A" },
                    { 2, "none@local", 1002, "Temp Employee Contact", "0000", "N/A" }
                });

            migrationBuilder.InsertData(
                table: "employeeleavebalances",
                columns: new[] { "EmployeeLeaveBalanceId", "AnnualAccrued", "AnnualUsed", "CarryOverAnnual", "EmployeeId", "SickAccrued", "SickUsed", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 0m, 0m, 0m, 1001, 0m, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 0m, 0m, 0m, 1002, 0m, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "supporttickets",
                columns: new[] { "Id", "AssignedToId", "CreatedAt", "EmployeeId", "Message", "Priority", "Status", "Subject", "UpdatedAt" },
                values: new object[] { 1, 1001, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1002, "This is a sample seeded ticket.", 0, 0, "Welcome to Support", null });

            migrationBuilder.InsertData(
                table: "supportmessages",
                columns: new[] { "Id", "AdminReply", "AdminReplyAt", "Body", "SenderEmployeeId", "SenderIsAdmin", "SentAt", "TicketId" },
                values: new object[,]
                {
                    { 1, null, null, "Hi, I need help setting up my account.", 1002, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, null, null, "Admin here — your account is now active!", 1001, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
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
                name: "IX_employeepayrollsummaries_EmployeeId",
                table: "employeepayrollsummaries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_employeepayrollsummaries_PayrollPeriodId_EmployeeId",
                table: "employeepayrollsummaries",
                columns: new[] { "PayrollPeriodId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_Email",
                table: "employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_EmployeeCode",
                table: "employees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_JobPositionId",
                table: "employees",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_PayGradeId",
                table: "employees",
                column: "PayGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_grievances_EmployeeId",
                table: "grievances",
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
                name: "IX_leaverequests_ApprovedByEmployeeEmployeeId",
                table: "leaverequests",
                column: "ApprovedByEmployeeEmployeeId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                name: "CalendarEvents");
========
=======
=======
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
>>>>>>> Stashed changes
=======
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
>>>>>>> Stashed changes
<<<<<<<< Updated upstream:NZFTC-EMS/Migrations/20251120090723_InitialBuild.cs
                name: "CalendarEvents");
========
========
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
========
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
>>>>>>> Stashed changes
=======
========
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
========
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs
>>>>>>> Stashed changes
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "calendarevents");
>>>>>>>> Stashed changes:NZFTC-EMS/Migrations/20251125090239_Announcement.cs

            migrationBuilder.DropTable(
                name: "employeeemergencycontacts");

            migrationBuilder.DropTable(
                name: "employeeleavebalances");

            migrationBuilder.DropTable(
                name: "employeepayrollsummaries");

            migrationBuilder.DropTable(
                name: "grievances");

            migrationBuilder.DropTable(
                name: "holidays");

            migrationBuilder.DropTable(
                name: "leaverequests");

            migrationBuilder.DropTable(
                name: "supportmessages");

            migrationBuilder.DropTable(
                name: "payrollperiods");

            migrationBuilder.DropTable(
                name: "supporttickets");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "jobpositions");

            migrationBuilder.DropTable(
                name: "paygrades");
        }
    }
}
