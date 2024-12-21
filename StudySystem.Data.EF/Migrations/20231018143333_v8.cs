using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudySystem.Data.EF.Migrations
{
    public partial class v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressUsers_districts_districts_code",
                table: "AddressUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressUsers_provinces_province_code",
                table: "AddressUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressUsers_wards_ward_code",
                table: "AddressUsers");

            migrationBuilder.RenameColumn(
                name: "ward_code",
                table: "AddressUsers",
                newName: "WardCode");

            migrationBuilder.RenameColumn(
                name: "province_code",
                table: "AddressUsers",
                newName: "ProvinceCode");

            migrationBuilder.RenameColumn(
                name: "districts_code",
                table: "AddressUsers",
                newName: "DistrictCode");

            migrationBuilder.RenameIndex(
                name: "IX_AddressUsers_ward_code",
                table: "AddressUsers",
                newName: "IX_AddressUsers_WardCode");

            migrationBuilder.RenameIndex(
                name: "IX_AddressUsers_UserID_ward_code_districts_code_province_code",
                table: "AddressUsers",
                newName: "IX_AddressUsers_UserID_WardCode_DistrictCode_ProvinceCode");

            migrationBuilder.RenameIndex(
                name: "IX_AddressUsers_province_code",
                table: "AddressUsers",
                newName: "IX_AddressUsers_ProvinceCode");

            migrationBuilder.RenameIndex(
                name: "IX_AddressUsers_districts_code",
                table: "AddressUsers",
                newName: "IX_AddressUsers_DistrictCode");

            migrationBuilder.AlterColumn<string>(
                name: "Descriptions",
                table: "AddressUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "WardCode",
                table: "AddressUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "ProvinceCode",
                table: "AddressUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "DistrictCode",
                table: "AddressUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressUsers_districts_DistrictCode",
                table: "AddressUsers",
                column: "DistrictCode",
                principalTable: "districts",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressUsers_provinces_ProvinceCode",
                table: "AddressUsers",
                column: "ProvinceCode",
                principalTable: "provinces",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressUsers_wards_WardCode",
                table: "AddressUsers",
                column: "WardCode",
                principalTable: "wards",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressUsers_districts_DistrictCode",
                table: "AddressUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressUsers_provinces_ProvinceCode",
                table: "AddressUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressUsers_wards_WardCode",
                table: "AddressUsers");

            migrationBuilder.RenameColumn(
                name: "WardCode",
                table: "AddressUsers",
                newName: "ward_code");

            migrationBuilder.RenameColumn(
                name: "ProvinceCode",
                table: "AddressUsers",
                newName: "province_code");

            migrationBuilder.RenameColumn(
                name: "DistrictCode",
                table: "AddressUsers",
                newName: "districts_code");

            migrationBuilder.RenameIndex(
                name: "IX_AddressUsers_WardCode",
                table: "AddressUsers",
                newName: "IX_AddressUsers_ward_code");

            migrationBuilder.RenameIndex(
                name: "IX_AddressUsers_UserID_WardCode_DistrictCode_ProvinceCode",
                table: "AddressUsers",
                newName: "IX_AddressUsers_UserID_ward_code_districts_code_province_code");

            migrationBuilder.RenameIndex(
                name: "IX_AddressUsers_ProvinceCode",
                table: "AddressUsers",
                newName: "IX_AddressUsers_province_code");

            migrationBuilder.RenameIndex(
                name: "IX_AddressUsers_DistrictCode",
                table: "AddressUsers",
                newName: "IX_AddressUsers_districts_code");

            migrationBuilder.AlterColumn<string>(
                name: "Descriptions",
                table: "AddressUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ward_code",
                table: "AddressUsers",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "province_code",
                table: "AddressUsers",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "districts_code",
                table: "AddressUsers",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressUsers_districts_districts_code",
                table: "AddressUsers",
                column: "districts_code",
                principalTable: "districts",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressUsers_provinces_province_code",
                table: "AddressUsers",
                column: "province_code",
                principalTable: "provinces",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressUsers_wards_ward_code",
                table: "AddressUsers",
                column: "ward_code",
                principalTable: "wards",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
