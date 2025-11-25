using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZFTC_EMS.Migrations
{
    /// <inheritdoc />
    public partial class SeedAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Body", "CreatedAt", "IsActive", "Title" },
                values: new object[] { 1, "Remember to complete your profile and update emergency contacts.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Welcome to NZFTC EMS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
