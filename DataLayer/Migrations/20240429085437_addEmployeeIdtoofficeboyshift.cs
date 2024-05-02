using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addEmployeeIdtoofficeboyshift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeBoyShifts_AspNetUsers_EmployeeId1",
                table: "OfficeBoyShifts");

            migrationBuilder.DropIndex(
                name: "IX_OfficeBoyShifts_EmployeeId1",
                table: "OfficeBoyShifts");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "OfficeBoyShifts");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "OfficeBoyShifts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeBoyShifts_EmployeeId",
                table: "OfficeBoyShifts",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeBoyShifts_AspNetUsers_EmployeeId",
                table: "OfficeBoyShifts",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeBoyShifts_AspNetUsers_EmployeeId",
                table: "OfficeBoyShifts");

            migrationBuilder.DropIndex(
                name: "IX_OfficeBoyShifts_EmployeeId",
                table: "OfficeBoyShifts");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "OfficeBoyShifts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId1",
                table: "OfficeBoyShifts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeBoyShifts_EmployeeId1",
                table: "OfficeBoyShifts",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeBoyShifts_AspNetUsers_EmployeeId1",
                table: "OfficeBoyShifts",
                column: "EmployeeId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
