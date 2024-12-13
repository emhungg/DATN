using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudySystem.Data.EF.Migrations
{
    public partial class v42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentBody",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ContentFooter",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ContentHeader",
                table: "News");

            migrationBuilder.DropColumn(
                name: "TitleBody",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "TitleFooter",
                table: "News",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "News",
                newName: "TitleFooter");

            migrationBuilder.AddColumn<string>(
                name: "ContentBody",
                table: "News",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentFooter",
                table: "News",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentHeader",
                table: "News",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleBody",
                table: "News",
                type: "text",
                nullable: true);
        }
    }
}
