using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZFTC_EMS.Migrations
{
    /// <inheritdoc />
    public partial class Seed_MoreDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employeeemergencycontacts_Employees_EmployeeId",
                table: "employeeemergencycontacts");

            migrationBuilder.DropForeignKey(
                name: "FK_employeeleavebalances_Employees_EmployeeId",
                table: "employeeleavebalances");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeePayrollSummaries_Employees_EmployeeId",
                table: "EmployeePayrollSummaries");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeePayrollSummaries_payrollperiods_PayrollPeriodId",
                table: "EmployeePayrollSummaries");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeePayrollSummaries_payrollruns_PayrollRunId",
                table: "EmployeePayrollSummaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_jobpositions_JobPositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_paygrades_PayGradeId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Grievances_Employees_EmployeeId",
                table: "Grievances");

            migrationBuilder.DropForeignKey(
                name: "FK_leaverequests_Employees_ApprovedByEmployeeId",
                table: "leaverequests");

            migrationBuilder.DropForeignKey(
                name: "FK_leaverequests_Employees_EmployeeId",
                table: "leaverequests");

            migrationBuilder.DropForeignKey(
                name: "FK_supporttickets_Employees_EmployeeId",
                table: "supporttickets");

            migrationBuilder.DropForeignKey(
                name: "FK_timesheetentries_Employees_EmployeeId",
                table: "timesheetentries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeePayrollSummaries",
                table: "EmployeePayrollSummaries");

            migrationBuilder.DeleteData(
                table: "Grievances",
                keyColumn: "GrievanceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Grievances",
                keyColumn: "GrievanceId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Grievances",
                keyColumn: "GrievanceId",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "employees");

            migrationBuilder.RenameTable(
                name: "EmployeePayrollSummaries",
                newName: "employeepayrollsummaries");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PayGradeId",
                table: "employees",
                newName: "IX_employees_PayGradeId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_JobPositionId",
                table: "employees",
                newName: "IX_employees_JobPositionId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeePayrollSummaries_PayrollRunId",
                table: "employeepayrollsummaries",
                newName: "IX_employeepayrollsummaries_PayrollRunId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeePayrollSummaries_PayrollPeriodId",
                table: "employeepayrollsummaries",
                newName: "IX_employeepayrollsummaries_PayrollPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeePayrollSummaries_EmployeeId",
                table: "employeepayrollsummaries",
                newName: "IX_employeepayrollsummaries_EmployeeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "employees",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "StudentLoan",
                table: "employeepayrollsummaries",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PayRate",
                table: "employeepayrollsummaries",
                type: "decimal(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PAYE",
                table: "employeepayrollsummaries",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetPay",
                table: "employeepayrollsummaries",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "KiwiSaverEmployer",
                table: "employeepayrollsummaries",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "KiwiSaverEmployee",
                table: "employeepayrollsummaries",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossPay",
                table: "employeepayrollsummaries",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Deductions",
                table: "employeepayrollsummaries",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ACCLevy",
                table: "employeepayrollsummaries",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employees",
                table: "employees",
                column: "EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employeepayrollsummaries",
                table: "employeepayrollsummaries",
                column: "EmployeePayrollSummaryId");

            migrationBuilder.InsertData(
                table: "calendarevents",
                columns: new[] { "Id", "Description", "End", "EventType", "IsPublicHoliday", "IsTodo", "OwnerUsername", "Start", "Title" },
                values: new object[,]
                {
                    { 2, "15-minute catch-up for IT team.", new DateTime(2025, 11, 21, 9, 15, 0, 0, DateTimeKind.Unspecified), 1, false, false, "sarah@nzftc.local", new DateTime(2025, 11, 21, 9, 0, 0, 0, DateTimeKind.Unspecified), "Daily Stand-up – IT" },
                    { 3, "Approve timesheets before 3 PM.", new DateTime(2025, 11, 19, 15, 30, 0, 0, DateTimeKind.Unspecified), 3, false, true, "admin@nzftc.local", new DateTime(2025, 11, 19, 15, 0, 0, 0, DateTimeKind.Unspecified), "Payroll Cut-off" },
                    { 4, "Sick leave", new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, false, false, "sarah@nzftc.local", new DateTime(2025, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved leave – Sarah Williams" },
                    { 5, "Annual leave", new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, false, false, "daniel@nzftc.local", new DateTime(2025, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved leave – Daniel Lee" }
                });

            migrationBuilder.UpdateData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 1,
                columns: new[] { "AnnualAccrued", "AnnualUsed", "SickAccrued", "SickUsed", "UpdatedAt" },
                values: new object[] { 10m, 2m, 5m, 1m, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 2,
                columns: new[] { "AnnualAccrued", "AnnualUsed", "SickAccrued", "UpdatedAt" },
                values: new object[] { 8m, 1m, 4m, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 4,
                columns: new[] { "CarryOverAnnual", "UpdatedAt" },
                values: new object[] { 1m, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "employeepayrollsummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 2,
                columns: new[] { "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PayRate", "PayrollRunId", "TotalHours" },
                values: new object[] { 12m, 202m, 1002, new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1330m, 40m, 40m, 1128m, 150m, 35.00m, 1, 38m });

            migrationBuilder.InsertData(
                table: "employeepayrollsummaries",
                columns: new[] { "EmployeePayrollSummaryId", "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PaidAt", "PayRate", "PayrollPeriodId", "PayrollRunId", "RateType", "Status", "StudentLoan", "TotalHours" },
                values: new object[,]
                {
                    { 3, 14m, 260m, 1003, new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1890m, 56m, 56m, 1630m, 190m, null, 45.00m, 1, 1, (byte)0, (byte)2, 0m, 42m },
                    { 4, 15m, 260m, 1001, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, null, 50.00m, 1, 2, (byte)0, (byte)0, 0m, 40m },
                    { 5, 17m, 344m, 1004, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2220m, 67m, 67m, 1876m, 260m, null, 60.00m, 1, 2, (byte)0, (byte)0, 0m, 37m }
                });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "EmployeeId", "Address", "Birthday", "Department", "Email", "EmployeeCode", "FirstName", "Gender", "JobPositionId", "LastName", "PasswordHash", "PasswordSalt", "PayFrequency", "PayGradeId", "Phone", "StartDate" },
                values: new object[,]
                {
                    { 1005, "8 Harbour View", new DateTime(1999, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operations", "olivia@nzftc.local", "NZFTC1005", "Olivia", "Female", 32, "Chen", new byte[0], new byte[0], (byte)0, 2, null, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1006, "55 Tech Lane", new DateTime(1996, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "IT", "daniel@nzftc.local", "NZFTC1006", "Daniel", "Male", 24, "Lee", new byte[0], new byte[0], (byte)0, 2, null, new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1007, "21 Ledger Street", new DateTime(1994, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", "emma@nzftc.local", "NZFTC1007", "Emma", "Female", 8, "Johnson", new byte[0], new byte[0], (byte)0, 2, null, new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1008, "3 Riverbank Road", new DateTime(1992, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operations", "liam@nzftc.local", "NZFTC1008", "Liam", "Male", 28, "Davis", new byte[0], new byte[0], (byte)0, 4, null, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PaidAt", "ProcessedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "payrollruns",
                columns: new[] { "PayrollRunId", "CreatedAt", "PaidAt", "PayFrequency", "PeriodEnd", "PeriodStart", "ProcessedAt", "Status" },
                values: new object[] { 3, new DateTime(2025, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, new DateTime(2025, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0 });

            migrationBuilder.InsertData(
                table: "supporttickets",
                columns: new[] { "Id", "AssignedToId", "CreatedAt", "EmployeeId", "Message", "Priority", "Status", "Subject", "UpdatedAt" },
                values: new object[] { 3, 1001, new DateTime(2025, 11, 18, 14, 0, 0, 0, DateTimeKind.Unspecified), 1004, "My overtime for last week is missing.", 2, 1, "Payslip amount seems incorrect", new DateTime(2025, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "timesheetentries",
                keyColumn: "TimesheetEntryId",
                keyValue: 2,
                column: "PayrollRunId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "timesheetentries",
                keyColumn: "TimesheetEntryId",
                keyValue: 5,
                column: "AdminNote",
                value: "Incorrect break time; please resubmit.");

            migrationBuilder.InsertData(
                table: "employeeleavebalances",
                columns: new[] { "EmployeeLeaveBalanceId", "AnnualAccrued", "AnnualUsed", "CarryOverAnnual", "EmployeeId", "SickAccrued", "SickUsed", "UpdatedAt" },
                values: new object[,]
                {
                    { 5, 6m, 0m, 0m, 1005, 3m, 0m, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 7m, 1m, 0m, 1006, 4m, 0m, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 9m, 2m, 0m, 1007, 5m, 1m, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 12m, 3m, 2m, 1008, 6m, 1m, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "employeepayrollsummaries",
                columns: new[] { "EmployeePayrollSummaryId", "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PaidAt", "PayRate", "PayrollPeriodId", "PayrollRunId", "RateType", "Status", "StudentLoan", "TotalHours" },
                values: new object[] { 6, 7m, 122m, 1005, new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 840m, 25m, 25m, 718m, 90m, null, 28.00m, 1, 3, (byte)0, (byte)0, 0m, 30m });

            migrationBuilder.InsertData(
                table: "leaverequests",
                columns: new[] { "LeaveRequestId", "ApprovedAt", "ApprovedByEmployeeId", "EmployeeId", "EndDate", "LeaveType", "Reason", "RequestedAt", "StartDate" },
                values: new object[] { 4, null, null, 1005, new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sick", "Migraine", new DateTime(2025, 11, 21, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "leaverequests",
                columns: new[] { "LeaveRequestId", "ApprovedAt", "ApprovedByEmployeeId", "EmployeeId", "EndDate", "LeaveType", "Reason", "RequestedAt", "StartDate", "Status" },
                values: new object[] { 5, new DateTime(2025, 11, 19, 16, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1006, new DateTime(2025, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual", "Short break", new DateTime(2025, 11, 19, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" });

            migrationBuilder.InsertData(
                table: "supportmessages",
                columns: new[] { "Id", "AdminReply", "AdminReplyAt", "Body", "SenderEmployeeId", "SenderIsAdmin", "SentAt", "TicketId" },
                values: new object[,]
                {
                    { 5, null, null, "My timesheet shows 42 hours but payslip only paid 40.", 1004, false, new DateTime(2025, 11, 18, 14, 5, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 6, null, null, "We’re checking your timesheet against the payroll run.", 1001, true, new DateTime(2025, 11, 19, 9, 10, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.InsertData(
                table: "supporttickets",
                columns: new[] { "Id", "AssignedToId", "CreatedAt", "EmployeeId", "Message", "Priority", "Status", "Subject", "UpdatedAt" },
                values: new object[] { 2, 1006, new DateTime(2025, 11, 21, 9, 30, 0, 0, DateTimeKind.Unspecified), 1005, "I get an error when submitting my timesheet for this week.", 2, 0, "Cannot submit timesheet", new DateTime(2025, 11, 21, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "timesheetentries",
                columns: new[] { "TimesheetEntryId", "AdminNote", "ApprovedAt", "BreakEndTime", "BreakStartTime", "CreatedAt", "EmployeeId", "FinishTime", "PayrollRunId", "StartTime", "Status", "SubmittedAt", "TotalHours", "WorkDate" },
                values: new object[,]
                {
                    { 6, null, new DateTime(2025, 11, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0), new TimeSpan(0, 12, 30, 0, 0), new DateTime(2025, 11, 14, 8, 0, 0, 0, DateTimeKind.Unspecified), 1005, new TimeSpan(0, 17, 0, 0, 0), 2, new TimeSpan(0, 8, 30, 0, 0), 2, new DateTime(2025, 11, 14, 17, 5, 0, 0, DateTimeKind.Unspecified), 8.0m, new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, null, null, new TimeSpan(0, 13, 30, 0, 0), new TimeSpan(0, 13, 0, 0, 0), new DateTime(2025, 11, 15, 8, 45, 0, 0, DateTimeKind.Unspecified), 1006, new TimeSpan(0, 18, 0, 0, 0), 2, new TimeSpan(0, 9, 0, 0, 0), 1, new DateTime(2025, 11, 15, 18, 10, 0, 0, DateTimeKind.Unspecified), 8.5m, new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, null, null, new TimeSpan(0, 13, 0, 0, 0), new TimeSpan(0, 12, 30, 0, 0), new DateTime(2025, 11, 16, 8, 40, 0, 0, DateTimeKind.Unspecified), 1007, new TimeSpan(0, 17, 30, 0, 0), null, new TimeSpan(0, 9, 0, 0, 0), 0, null, 8.0m, new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Approved – training coverage.", new DateTime(2025, 11, 22, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 30, 0, 0), new TimeSpan(0, 12, 0, 0, 0), new DateTime(2025, 11, 21, 7, 50, 0, 0, DateTimeKind.Unspecified), 1008, new TimeSpan(0, 16, 30, 0, 0), 3, new TimeSpan(0, 8, 0, 0, 0), 2, new DateTime(2025, 11, 21, 16, 40, 0, 0, DateTimeKind.Unspecified), 8.0m, new DateTime(2025, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "supportmessages",
                columns: new[] { "Id", "AdminReply", "AdminReplyAt", "Body", "SenderEmployeeId", "SenderIsAdmin", "SentAt", "TicketId" },
                values: new object[,]
                {
                    { 3, null, null, "I get a red error banner when I click submit.", 1005, false, new DateTime(2025, 11, 21, 9, 32, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 4, null, null, "Can you send me a screenshot of the error?", 1006, true, new DateTime(2025, 11, 21, 9, 45, 0, 0, DateTimeKind.Unspecified), 2 }
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_employeeemergencycontacts_employees_EmployeeId",
                table: "employeeemergencycontacts",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employeeleavebalances_employees_EmployeeId",
                table: "employeeleavebalances",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employeepayrollsummaries_employees_EmployeeId",
                table: "employeepayrollsummaries",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employeepayrollsummaries_payrollperiods_PayrollPeriodId",
                table: "employeepayrollsummaries",
                column: "PayrollPeriodId",
                principalTable: "payrollperiods",
                principalColumn: "PayrollPeriodId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_employeepayrollsummaries_payrollruns_PayrollRunId",
                table: "employeepayrollsummaries",
                column: "PayrollRunId",
                principalTable: "payrollruns",
                principalColumn: "PayrollRunId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_employees_jobpositions_JobPositionId",
                table: "employees",
                column: "JobPositionId",
                principalTable: "jobpositions",
                principalColumn: "JobPositionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_employees_paygrades_PayGradeId",
                table: "employees",
                column: "PayGradeId",
                principalTable: "paygrades",
                principalColumn: "PayGradeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grievances_employees_EmployeeId",
                table: "Grievances",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_leaverequests_employees_ApprovedByEmployeeId",
                table: "leaverequests",
                column: "ApprovedByEmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_leaverequests_employees_EmployeeId",
                table: "leaverequests",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supporttickets_employees_EmployeeId",
                table: "supporttickets",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_timesheetentries_employees_EmployeeId",
                table: "timesheetentries",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employeeemergencycontacts_employees_EmployeeId",
                table: "employeeemergencycontacts");

            migrationBuilder.DropForeignKey(
                name: "FK_employeeleavebalances_employees_EmployeeId",
                table: "employeeleavebalances");

            migrationBuilder.DropForeignKey(
                name: "FK_employeepayrollsummaries_employees_EmployeeId",
                table: "employeepayrollsummaries");

            migrationBuilder.DropForeignKey(
                name: "FK_employeepayrollsummaries_payrollperiods_PayrollPeriodId",
                table: "employeepayrollsummaries");

            migrationBuilder.DropForeignKey(
                name: "FK_employeepayrollsummaries_payrollruns_PayrollRunId",
                table: "employeepayrollsummaries");

            migrationBuilder.DropForeignKey(
                name: "FK_employees_jobpositions_JobPositionId",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "FK_employees_paygrades_PayGradeId",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Grievances_employees_EmployeeId",
                table: "Grievances");

            migrationBuilder.DropForeignKey(
                name: "FK_leaverequests_employees_ApprovedByEmployeeId",
                table: "leaverequests");

            migrationBuilder.DropForeignKey(
                name: "FK_leaverequests_employees_EmployeeId",
                table: "leaverequests");

            migrationBuilder.DropForeignKey(
                name: "FK_supporttickets_employees_EmployeeId",
                table: "supporttickets");

            migrationBuilder.DropForeignKey(
                name: "FK_timesheetentries_employees_EmployeeId",
                table: "timesheetentries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employees",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_Email",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_EmployeeCode",
                table: "employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employeepayrollsummaries",
                table: "employeepayrollsummaries");

            migrationBuilder.DeleteData(
                table: "calendarevents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "calendarevents",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "calendarevents",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "calendarevents",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "employeepayrollsummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "employeepayrollsummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "employeepayrollsummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "employeepayrollsummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "leaverequests",
                keyColumn: "LeaveRequestId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "leaverequests",
                keyColumn: "LeaveRequestId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "supportmessages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "supportmessages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "supportmessages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "supportmessages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "timesheetentries",
                keyColumn: "TimesheetEntryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "timesheetentries",
                keyColumn: "TimesheetEntryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "timesheetentries",
                keyColumn: "TimesheetEntryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "timesheetentries",
                keyColumn: "TimesheetEntryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "supporttickets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "supporttickets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1005);

            migrationBuilder.RenameTable(
                name: "employees",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "employeepayrollsummaries",
                newName: "EmployeePayrollSummaries");

            migrationBuilder.RenameIndex(
                name: "IX_employees_PayGradeId",
                table: "Employees",
                newName: "IX_Employees_PayGradeId");

            migrationBuilder.RenameIndex(
                name: "IX_employees_JobPositionId",
                table: "Employees",
                newName: "IX_Employees_JobPositionId");

            migrationBuilder.RenameIndex(
                name: "IX_employeepayrollsummaries_PayrollRunId",
                table: "EmployeePayrollSummaries",
                newName: "IX_EmployeePayrollSummaries_PayrollRunId");

            migrationBuilder.RenameIndex(
                name: "IX_employeepayrollsummaries_PayrollPeriodId",
                table: "EmployeePayrollSummaries",
                newName: "IX_EmployeePayrollSummaries_PayrollPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_employeepayrollsummaries_EmployeeId",
                table: "EmployeePayrollSummaries",
                newName: "IX_EmployeePayrollSummaries_EmployeeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Employees",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<decimal>(
                name: "StudentLoan",
                table: "EmployeePayrollSummaries",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "PayRate",
                table: "EmployeePayrollSummaries",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldPrecision: 12,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "PAYE",
                table: "EmployeePayrollSummaries",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "NetPay",
                table: "EmployeePayrollSummaries",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "KiwiSaverEmployer",
                table: "EmployeePayrollSummaries",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "KiwiSaverEmployee",
                table: "EmployeePayrollSummaries",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossPay",
                table: "EmployeePayrollSummaries",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Deductions",
                table: "EmployeePayrollSummaries",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ACCLevy",
                table: "EmployeePayrollSummaries",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeePayrollSummaries",
                table: "EmployeePayrollSummaries",
                column: "EmployeePayrollSummaryId");

            migrationBuilder.UpdateData(
                table: "EmployeePayrollSummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 2,
                columns: new[] { "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PayRate", "PayrollRunId", "TotalHours" },
                values: new object[] { 15m, 260m, 1001, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, 50.00m, 2, 40m });

            migrationBuilder.InsertData(
                table: "Grievances",
                columns: new[] { "GrievanceId", "AdminResponse", "EmployeeId", "EmployeeMessage", "Status", "Subject", "SubmittedAt" },
                values: new object[,]
                {
                    { 1, null, 1002, "My roster has been changed without notice and clashes with study.", (byte)0, "Roster concerns", new DateTime(2025, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "IT has been notified and will replace your workstation this week.", 1003, "My workstation keeps freezing and affects my productivity.", (byte)1, "Equipment not working", new DateTime(2025, 11, 18, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "We have recalculated and processed an adjustment in your next pay.", 1004, "I believe my overtime for October was underpaid.", (byte)3, "Payroll discrepancy – October", new DateTime(2025, 11, 10, 14, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 1,
                columns: new[] { "AnnualAccrued", "AnnualUsed", "SickAccrued", "SickUsed", "UpdatedAt" },
                values: new object[] { 0m, 0m, 0m, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 2,
                columns: new[] { "AnnualAccrued", "AnnualUsed", "SickAccrued", "UpdatedAt" },
                values: new object[] { 0m, 0m, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "employeeleavebalances",
                keyColumn: "EmployeeLeaveBalanceId",
                keyValue: 4,
                columns: new[] { "CarryOverAnnual", "UpdatedAt" },
                values: new object[] { 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PaidAt", "ProcessedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "timesheetentries",
                keyColumn: "TimesheetEntryId",
                keyValue: 2,
                column: "PayrollRunId",
                value: null);

            migrationBuilder.UpdateData(
                table: "timesheetentries",
                keyColumn: "TimesheetEntryId",
                keyValue: 5,
                column: "AdminNote",
                value: "Rejected – incorrect break time; please resubmit.");

            migrationBuilder.AddForeignKey(
                name: "FK_employeeemergencycontacts_Employees_EmployeeId",
                table: "employeeemergencycontacts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employeeleavebalances_Employees_EmployeeId",
                table: "employeeleavebalances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeePayrollSummaries_Employees_EmployeeId",
                table: "EmployeePayrollSummaries",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeePayrollSummaries_payrollperiods_PayrollPeriodId",
                table: "EmployeePayrollSummaries",
                column: "PayrollPeriodId",
                principalTable: "payrollperiods",
                principalColumn: "PayrollPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeePayrollSummaries_payrollruns_PayrollRunId",
                table: "EmployeePayrollSummaries",
                column: "PayrollRunId",
                principalTable: "payrollruns",
                principalColumn: "PayrollRunId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_jobpositions_JobPositionId",
                table: "Employees",
                column: "JobPositionId",
                principalTable: "jobpositions",
                principalColumn: "JobPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_paygrades_PayGradeId",
                table: "Employees",
                column: "PayGradeId",
                principalTable: "paygrades",
                principalColumn: "PayGradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grievances_Employees_EmployeeId",
                table: "Grievances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_leaverequests_Employees_ApprovedByEmployeeId",
                table: "leaverequests",
                column: "ApprovedByEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_leaverequests_Employees_EmployeeId",
                table: "leaverequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supporttickets_Employees_EmployeeId",
                table: "supporttickets",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_timesheetentries_Employees_EmployeeId",
                table: "timesheetentries",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
