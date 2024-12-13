using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudySystem.Data.EF.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "VerificationOTPs",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "UserTokens",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "UserDetails",
                newName: "UserFullName");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "UserDetails",
                newName: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "VerificationOTPs",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserTokens",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "UserFullName",
                table: "UserDetails",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserDetails",
                newName: "Username");
        }
    }
}
