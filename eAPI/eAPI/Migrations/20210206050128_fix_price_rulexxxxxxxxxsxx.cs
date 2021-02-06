using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_price_rulexxxxxxxxxsxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_vendor_Provinces_province_id",
                table: "tbl_vendor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces");

            migrationBuilder.RenameTable(
                name: "Provinces",
                newName: "tbl_province");

            migrationBuilder.AddColumn<int>(
                name: "VendorGroupModelid",
                table: "tbl_vendor",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_province",
                table: "tbl_province",
                column: "id");

            migrationBuilder.CreateTable(
                name: "tbl_vendor_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vendor_group_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    vendor_group_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_vendor_group", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_vendor_VendorGroupModelid",
                table: "tbl_vendor",
                column: "VendorGroupModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_vendor_tbl_province_province_id",
                table: "tbl_vendor",
                column: "province_id",
                principalTable: "tbl_province",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_vendor_tbl_vendor_group_VendorGroupModelid",
                table: "tbl_vendor",
                column: "VendorGroupModelid",
                principalTable: "tbl_vendor_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_vendor_tbl_province_province_id",
                table: "tbl_vendor");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_vendor_tbl_vendor_group_VendorGroupModelid",
                table: "tbl_vendor");

            migrationBuilder.DropTable(
                name: "tbl_vendor_group");

            migrationBuilder.DropIndex(
                name: "IX_tbl_vendor_VendorGroupModelid",
                table: "tbl_vendor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_province",
                table: "tbl_province");

            migrationBuilder.DropColumn(
                name: "VendorGroupModelid",
                table: "tbl_vendor");

            migrationBuilder.RenameTable(
                name: "tbl_province",
                newName: "Provinces");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_vendor_Provinces_province_id",
                table: "tbl_vendor",
                column: "province_id",
                principalTable: "Provinces",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
