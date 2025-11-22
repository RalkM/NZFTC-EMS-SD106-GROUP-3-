using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZFTC_EMS.Migrations
{
    /// <inheritdoc />
    public partial class AddPayrollHoursFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BreakMinutes",
                table: "employeepayrollsummaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "employeepayrollsummaries",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "employeepayrollsummaries",
                type: "time(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakMinutes",
                table: "employeepayrollsummaries");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "employeepayrollsummaries");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "employeepayrollsummaries");
        }
    }
}
