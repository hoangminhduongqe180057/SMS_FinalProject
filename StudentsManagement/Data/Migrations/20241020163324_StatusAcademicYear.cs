using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsManagement.Migrations
{
    /// <inheritdoc />
    public partial class StatusAcademicYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicYears_SystemCodeDetails_StatusId",
                table: "AcademicYears");

            migrationBuilder.DropIndex(
                name: "IX_AcademicYears_StatusId",
                table: "AcademicYears");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "AcademicYears");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AcademicYears",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AcademicYears");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "AcademicYears",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_StatusId",
                table: "AcademicYears",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicYears_SystemCodeDetails_StatusId",
                table: "AcademicYears",
                column: "StatusId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
