using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class update0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "tbl_product_modifier",
                newName: "ProductModifierModelid");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier",
                newName: "IX_tbl_product_modifier_ProductModifierModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier",
                column: "ProductModifierModelid",
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
