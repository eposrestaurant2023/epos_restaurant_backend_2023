using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_modifer_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "modifier_group_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "modifier_group_id",
                table: "tbl_modifier",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_modifier_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modifier_group_name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_modifier_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_modifier_group_product_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modifer_group_id = table.Column<int>(type: "int", nullable: false),
                    product_category_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_modifier_group_product_category", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_modifier_group_product_category_tbl_modifier_group_modifer_group_id",
                        column: x => x.modifer_group_id,
                        principalTable: "tbl_modifier_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_modifier_group_product_category_tbl_product_category_product_category_id",
                        column: x => x.product_category_id,
                        principalTable: "tbl_product_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_modifier_group_id",
                table: "tbl_modifier",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_product_category_modifer_group_id",
                table: "tbl_modifier_group_product_category",
                column: "modifer_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_product_category_product_category_id",
                table: "tbl_modifier_group_product_category",
                column: "product_category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier");

            migrationBuilder.DropTable(
                name: "tbl_modifier_group_product_category");

            migrationBuilder.DropTable(
                name: "tbl_modifier_group");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_modifier_group_id",
                table: "tbl_modifier");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_modifier");
        }
    }
}
