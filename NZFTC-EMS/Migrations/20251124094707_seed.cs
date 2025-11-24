using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZFTC_EMS.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "payrollruns",
                columns: new[] { "PayrollRunId", "CreatedAt", "PaidAt", "PayFrequency", "PeriodEnd", "PeriodStart", "ProcessedAt", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 2, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1 }
                });

            migrationBuilder.InsertData(
                table: "EmployeePayrollSummaries",
                columns: new[] { "EmployeePayrollSummaryId", "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PaidAt", "PayRate", "PayrollPeriodId", "PayrollRunId", "RateType", "Status", "StudentLoan", "TotalHours" },
                values: new object[,]
                {
                    { 1, 15m, 260m, 1001, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1120m, 45m, 45m, 0m, 200m, null, 28.00m, 1, 1, (byte)0, (byte)2, 0m, 40m },
                    { 2, 60m, 1160m, 1003, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 5120m, 150m, 150m, 0m, 950m, null, 32.00m, 1, 2, (byte)0, (byte)1, 0m, 160m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmployeePayrollSummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmployeePayrollSummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 2);
        }
    }
}
