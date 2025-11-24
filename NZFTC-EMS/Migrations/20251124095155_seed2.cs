using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZFTC_EMS.Migrations
{
    /// <inheritdoc />
    public partial class seed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 1,
                columns: new[] { "PeriodEnd", "PeriodStart" },
                values: new object[] { new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 2,
                columns: new[] { "PeriodEnd", "PeriodStart" },
                values: new object[] { new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 1,
                columns: new[] { "PeriodEnd", "PeriodStart" },
                values: new object[] { new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "payrollruns",
                keyColumn: "PayrollRunId",
                keyValue: 2,
                columns: new[] { "PeriodEnd", "PeriodStart" },
                values: new object[] { new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
