using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZFTC_EMS.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeePhotoBytes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoBytes",
                table: "employees",
                type: "LONGBLOB",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1001,
                column: "PhotoBytes",
                value: null);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1002,
                column: "PhotoBytes",
                value: null);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1003,
                column: "PhotoBytes",
                value: null);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1004,
                column: "PhotoBytes",
                value: null);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1005,
                column: "PhotoBytes",
                value: null);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1006,
                column: "PhotoBytes",
                value: null);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1007,
                column: "PhotoBytes",
                value: null);

            migrationBuilder.UpdateData(
                table: "employees",
                keyColumn: "EmployeeId",
                keyValue: 1008,
                column: "PhotoBytes",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoBytes",
                table: "employees");
        }
    }
}
