using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class modifier_categoryxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_category_modifier_category_id",
                table: "tbl_modifier");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropTable(
                name: "tbl_modifier_category");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_modifier_category_id",
                table: "tbl_modifier");

            migrationBuilder.DropColumn(
                name: "modifier_category_id",
                table: "tbl_modifier");

            migrationBuilder.AlterColumn<int>(
                name: "modifier_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "is_multiple_select",
                table: "tbl_product_modifier",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_required",
                table: "tbl_product_modifier",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_section",
                table: "tbl_product_modifier",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "section_name",
                table: "tbl_product_modifier",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "is_multiple_select",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "is_required",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "is_section",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "section_name",
                table: "tbl_product_modifier");

            migrationBuilder.AlterColumn<int>(
                name: "modifier_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "modifier_category_id",
                table: "tbl_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbl_modifier_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    is_multiple_select = table.Column<bool>(type: "bit", nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    modifier_category_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_modifier_category", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_modifier_category_id",
                table: "tbl_modifier",
                column: "modifier_category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_category_modifier_category_id",
                table: "tbl_modifier",
                column: "modifier_category_id",
                principalTable: "tbl_modifier_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
