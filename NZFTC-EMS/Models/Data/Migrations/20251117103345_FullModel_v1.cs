using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZFTC_EMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class FullModel_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "supportmessages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "AdminReplyAt",
                table: "supportmessages",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SenderIsAdmin",
                table: "supportmessages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PayGrades",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AccessRole",
                table: "JobPositions",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "JobPositions",
                type: "varchar(80)",
                maxLength: 80,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PayGradeId",
                table: "JobPositions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Employees",
                type: "varchar(80)",
                maxLength: 80,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "Employees",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
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
                        name: "FK_employeeleavebalances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_supporttickets_EmployeeId",
                table: "supporttickets",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPositions_PayGradeId",
                table: "JobPositions",
                column: "PayGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employeeleavebalances_EmployeeId",
                table: "employeeleavebalances",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPositions_PayGrades_PayGradeId",
                table: "JobPositions",
                column: "PayGradeId",
                principalTable: "PayGrades",
                principalColumn: "PayGradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_supporttickets_Employees_EmployeeId",
                table: "supporttickets",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPositions_PayGrades_PayGradeId",
                table: "JobPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_supporttickets_Employees_EmployeeId",
                table: "supporttickets");

            migrationBuilder.DropTable(
                name: "employeeleavebalances");

            migrationBuilder.DropIndex(
                name: "IX_supporttickets_EmployeeId",
                table: "supporttickets");

            migrationBuilder.DropIndex(
                name: "IX_JobPositions_PayGradeId",
                table: "JobPositions");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "supportmessages");

            migrationBuilder.DropColumn(
                name: "AdminReplyAt",
                table: "supportmessages");

            migrationBuilder.DropColumn(
                name: "SenderIsAdmin",
                table: "supportmessages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PayGrades");

            migrationBuilder.DropColumn(
                name: "AccessRole",
                table: "JobPositions");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "JobPositions");

            migrationBuilder.DropColumn(
                name: "PayGradeId",
                table: "JobPositions");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "Employees");
        }
    }
}
