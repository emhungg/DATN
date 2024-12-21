using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudySystem.Data.EF.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "UserDetails");

            migrationBuilder.CreateTable(
                name: "administrative_regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    CodeName = table.Column<string>(type: "text", nullable: false),
                    CodeNameEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrative_regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "administrative_units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    FullNameEn = table.Column<string>(type: "text", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: false),
                    ShortNameEn = table.Column<string>(type: "text", nullable: false),
                    CodeName = table.Column<string>(type: "text", nullable: false),
                    CodeNameEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrative_units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "provinces",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NameEn = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    FullNameEn = table.Column<string>(type: "text", nullable: true),
                    CodeName = table.Column<string>(type: "text", nullable: true),
                    AdministrativeUnitId = table.Column<int>(type: "integer", nullable: true),
                    AdministrativeRegionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provinces", x => x.Code);
                    table.ForeignKey(
                        name: "FK_provinces_administrative_regions_AdministrativeRegionId",
                        column: x => x.AdministrativeRegionId,
                        principalTable: "administrative_regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_provinces_administrative_units_AdministrativeUnitId",
                        column: x => x.AdministrativeUnitId,
                        principalTable: "administrative_units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NameEn = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    FullNameEn = table.Column<string>(type: "text", nullable: true),
                    CodeName = table.Column<string>(type: "text", nullable: true),
                    ProvinceCode = table.Column<string>(type: "text", nullable: false),
                    AdministrativeUnitId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_districts", x => x.Code);
                    table.ForeignKey(
                        name: "FK_districts_administrative_units_AdministrativeUnitId",
                        column: x => x.AdministrativeUnitId,
                        principalTable: "administrative_units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_districts_provinces_ProvinceCode",
                        column: x => x.ProvinceCode,
                        principalTable: "provinces",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "wards",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NameEn = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    FullNameEn = table.Column<string>(type: "text", nullable: true),
                    CodeName = table.Column<string>(type: "text", nullable: true),
                    DistrictCode = table.Column<string>(type: "text", nullable: false),
                    AdministrativeUnitId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wards", x => x.Code);
                    table.ForeignKey(
                        name: "FK_wards_administrative_units_AdministrativeUnitId",
                        column: x => x.AdministrativeUnitId,
                        principalTable: "administrative_units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_wards_districts_DistrictCode",
                        column: x => x.DistrictCode,
                        principalTable: "districts",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Descriptions = table.Column<string>(type: "text", nullable: false),
                    ward_code = table.Column<string>(type: "varchar(20)", nullable: false),
                    districts_code = table.Column<string>(type: "varchar(20)", nullable: false),
                    province_code = table.Column<string>(type: "varchar(20)", nullable: false),
                    UserID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    CreateUser = table.Column<string>(type: "text", nullable: false),
                    UpdateUser = table.Column<string>(type: "text", nullable: false),
                    CreateDateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressUsers_districts_districts_code",
                        column: x => x.districts_code,
                        principalTable: "districts",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddressUsers_provinces_province_code",
                        column: x => x.province_code,
                        principalTable: "provinces",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddressUsers_UserDetails_UserID",
                        column: x => x.UserID,
                        principalTable: "UserDetails",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AddressUsers_wards_ward_code",
                        column: x => x.ward_code,
                        principalTable: "wards",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDetail_UserID",
                table: "UserDetails",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AddressUsers_districts_code",
                table: "AddressUsers",
                column: "districts_code");

            migrationBuilder.CreateIndex(
                name: "IX_AddressUsers_province_code",
                table: "AddressUsers",
                column: "province_code");

            migrationBuilder.CreateIndex(
                name: "IX_AddressUsers_UserID",
                table: "AddressUsers",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressUsers_UserID_ward_code_districts_code_province_code",
                table: "AddressUsers",
                columns: new[] { "UserID", "ward_code", "districts_code", "province_code" });

            migrationBuilder.CreateIndex(
                name: "IX_AddressUsers_ward_code",
                table: "AddressUsers",
                column: "ward_code");

            migrationBuilder.CreateIndex(
                name: "IX_districts_AdministrativeUnitId",
                table: "districts",
                column: "AdministrativeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_districts_ProvinceCode_AdministrativeUnitId",
                table: "districts",
                columns: new[] { "ProvinceCode", "AdministrativeUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_provinces_AdministrativeRegionId_AdministrativeUnitId",
                table: "provinces",
                columns: new[] { "AdministrativeRegionId", "AdministrativeUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_provinces_AdministrativeUnitId",
                table: "provinces",
                column: "AdministrativeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_wards_AdministrativeUnitId",
                table: "wards",
                column: "AdministrativeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_wards_DistrictCode_AdministrativeUnitId",
                table: "wards",
                columns: new[] { "DistrictCode", "AdministrativeUnitId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressUsers");

            migrationBuilder.DropTable(
                name: "wards");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "provinces");

            migrationBuilder.DropTable(
                name: "administrative_regions");

            migrationBuilder.DropTable(
                name: "administrative_units");

            migrationBuilder.DropIndex(
                name: "IX_UserDetail_UserID",
                table: "UserDetails");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "UserDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
