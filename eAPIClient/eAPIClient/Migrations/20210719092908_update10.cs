using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class update10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_ProductModifierModelid_test",
                table: "tbl_product_modifier");

            migrationBuilder.RenameColumn(
                name: "ProductModifierModelid_test",
                table: "tbl_product_modifier",
                newName: "ProductModifierModelid");

            migrationBuilder.RenameColumn(
                name: "id_test",
                table: "tbl_product_modifier",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_product_modifier_ProductModifierModelid_test",
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
                newName: "ProductModifierModelid_test");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_product_modifier",
                newName: "id_test");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier",
                newName: "IX_tbl_product_modifier_ProductModifierModelid_test");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_ProductModifierModelid_test",
                table: "tbl_product_modifier",
                column: "ProductModifierModelid_test",
                principalTable: "tbl_product_modifier",
                principalColumn: "id_test",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
