using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_modifier_guid_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier");

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
                name: "FK_tbl_modifier_group_product_category_tbl_modifier_group_modifer_group_id",
                table: "tbl_modifier_group_product_category");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_ingredient_tbl_modifier_modifier_id",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_modifier_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_modifier_ingredient",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_product_category_modifer_group_id",
                table: "tbl_modifier_group_product_category");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_modifier_group_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_modifier_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_parent_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_modifier_group_id",
                table: "tbl_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_modifier_group_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_modifier_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_modifier_group_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_modifier_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "product_modifier_id",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropColumn(
                name: "modifier_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "tbl_product_modifier");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
