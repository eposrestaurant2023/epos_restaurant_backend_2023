using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_modifer_guid1xxxxxx_21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_product_modifier_id",
                table: "tbl_product_modifier",
                column: "parent_id",
                principalTable: "tbl_product_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.RenameColumn(
                name: "ProductModifierModelid",
                table: "tbl_product_modifier",
                newName: "parent_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier",
                newName: "IX_tbl_product_modifier_parent_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier",
                column: "parent_id",
                principalTable: "tbl_product_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
