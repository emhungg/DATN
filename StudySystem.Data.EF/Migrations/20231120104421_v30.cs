using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudySystem.Data.EF.Migrations
{
    public partial class v30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Images",
            //    columns: table => new
            //    {
            //        ProductId = table.Column<Guid>(type: "text", nullable: false),
            //        ImageDes = table.Column<string>(type: "text", nullable: true),
            //        CreateUser = table.Column<string>(type: "text", nullable: false),
            //        UpdateUser = table.Column<string>(type: "text", nullable: false),
            //        CreateDateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        UpdateDateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Images", x => x.ProductId);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Images");
        }
    }
}
