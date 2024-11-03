using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsManagement.Migrations
{
    /// <inheritdoc />
    public partial class ClassStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Students",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActionStatusId",
                table: "ComplaintNotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintNotes_ActionStatusId",
                table: "ComplaintNotes",
                column: "ActionStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintNotes_SystemCodeDetails_ActionStatusId",
                table: "ComplaintNotes",
                column: "ActionStatusId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_SystemCodeDetails_ClassId",
                table: "Students",
                column: "ClassId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintNotes_SystemCodeDetails_ActionStatusId",
                table: "ComplaintNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_SystemCodeDetails_ClassId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ClassId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_ComplaintNotes_ActionStatusId",
                table: "ComplaintNotes");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ActionStatusId",
                table: "ComplaintNotes");
        }
    }
}
