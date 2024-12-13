using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudySystem.Data.EF.Migrations
{
    public partial class v27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductConfigurations",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    Chip = table.Column<string>(type: "text", nullable: false),
                    Ram = table.Column<int>(type: "integer", nullable: false),
                    Rom = table.Column<int>(type: "integer", nullable: false),
                    Screen = table.Column<int>(type: "integer", nullable: false),
                    CameraFont = table.Column<string>(type: "text", nullable: false),
                    CamerRear = table.Column<string>(type: "text", nullable: false),
                    Pin = table.Column<int>(type: "integer", nullable: false),
                    Charge = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductConfigurations", x => x.ProductId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductConfigurations");
        }
    }
}
