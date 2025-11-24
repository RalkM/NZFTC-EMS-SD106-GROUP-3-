using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZFTC_EMS.Migrations
{
    /// <inheritdoc />
    public partial class seed3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeePayrollSummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 1,
                columns: new[] { "GeneratedAt", "GrossPay", "NetPay", "PayRate" },
                values: new object[] { new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 1740m, 50.00m });

            migrationBuilder.UpdateData(
                table: "EmployeePayrollSummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 2,
                columns: new[] { "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PayRate", "Status", "TotalHours" },
                values: new object[] { 15m, 260m, 1001, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000m, 45m, 45m, 1740m, 200m, 50.00m, (byte)2, 40m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeePayrollSummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 1,
                columns: new[] { "GeneratedAt", "GrossPay", "NetPay", "PayRate" },
                values: new object[] { new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1120m, 0m, 28.00m });

            migrationBuilder.UpdateData(
                table: "EmployeePayrollSummaries",
                keyColumn: "EmployeePayrollSummaryId",
                keyValue: 2,
                columns: new[] { "ACCLevy", "Deductions", "EmployeeId", "GeneratedAt", "GrossPay", "KiwiSaverEmployee", "KiwiSaverEmployer", "NetPay", "PAYE", "PayRate", "Status", "TotalHours" },
                values: new object[] { 60m, 1160m, 1003, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 5120m, 150m, 150m, 0m, 950m, 32.00m, (byte)1, 160m });
        }
    }
}
