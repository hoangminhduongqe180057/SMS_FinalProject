using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentsManagement.Migrations
{
    /// <inheritdoc />
    public partial class ClassParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Parents",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Parents_ClassId",
                table: "Parents",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_SystemCodeDetails_ClassId",
                table: "Parents",
                column: "ClassId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parents_SystemCodeDetails_ClassId",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Parents_ClassId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Parents");
        }
    }
}
