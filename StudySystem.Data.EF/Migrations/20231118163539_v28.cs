using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudySystem.Data.EF.Migrations
{
    public partial class v28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CamerRear",
                table: "ProductConfigurations");

            migrationBuilder.DropColumn(
                name: "CameraFont",
                table: "ProductConfigurations");

            migrationBuilder.DropColumn(
                name: "Charge",
                table: "ProductConfigurations");

            migrationBuilder.DropColumn(
                name: "Pin",
                table: "ProductConfigurations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CamerRear",
                table: "ProductConfigurations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CameraFont",
                table: "ProductConfigurations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Charge",
                table: "ProductConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pin",
                table: "ProductConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
