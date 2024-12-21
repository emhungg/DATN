using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudySystem.Data.EF.Migrations
{
    public partial class v22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleHeader = table.Column<string>(type: "text", nullable: true),
                    ImageNew = table.Column<string>(type: "text", nullable: true),
                    ContentHeader = table.Column<string>(type: "text", nullable: true),
                    TitleBody = table.Column<string>(type: "text", nullable: true),
                    ContentBody = table.Column<string>(type: "text", nullable: true),
                    TitleFooter = table.Column<string>(type: "text", nullable: true),
                    ContentFooter = table.Column<string>(type: "text", nullable: true),
                    CreateUser = table.Column<string>(type: "text", nullable: false),
                    UpdateUser = table.Column<string>(type: "text", nullable: false),
                    CreateDateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
