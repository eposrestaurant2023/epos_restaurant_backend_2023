using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class kitchen_group_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "kitchen_group_id",
                table: "tbl_product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_kitchen_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kitchen_group_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kitchen_group", x => x.id);
                });



            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_kitchen_group_id",
                table: "tbl_product",
                column: "kitchen_group_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_item_parent_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_modifier_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_kitchen_group_kitchen_group_id",
                table: "tbl_product");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_product_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropTable(
                name: "tbl_kitchen_group");

            migrationBuilder.DropTable(
                name: "tbl_modifier_ingredient");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_kitchen_group_id",
                table: "tbl_product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_modifier_group_item",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_modifier_group_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_parent_id",
                table: "tbl_modifier_group_item");

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

            migrationBuilder.DropColumn(
                name: "kitchen_group_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "is_multiple_select",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "is_required",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "is_section",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "section_name",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_modifier_group_item");

            migrationBuilder.AlterColumn<int>(
                name: "product_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "modifier_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "modifier_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "modifier_group_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_modifier_group_item",
                table: "tbl_modifier_group_item",
                columns: new[] { "modifier_group_id", "modifier_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier_group_item",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_modifier_id",
                table: "tbl_modifier_group_item",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_product_id",
                table: "tbl_product_modifier",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
