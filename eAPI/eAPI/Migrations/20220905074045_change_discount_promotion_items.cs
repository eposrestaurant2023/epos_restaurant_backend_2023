using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_discount_promotion_items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_discount_promotion_item_tbl_discount_promotion_dicount_promotion_id",
                table: "tbl_discount_promotion_item");

            migrationBuilder.RenameColumn(
                name: "dicount_promotion_id",
                table: "tbl_discount_promotion_item",
                newName: "discount_promotion_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_discount_promotion_item_dicount_promotion_id",
                table: "tbl_discount_promotion_item",
                newName: "IX_tbl_discount_promotion_item_discount_promotion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_discount_promotion_item_tbl_discount_promotion_discount_promotion_id",
                table: "tbl_discount_promotion_item",
                column: "discount_promotion_id",
                principalTable: "tbl_discount_promotion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_discount_promotion_item_tbl_discount_promotion_discount_promotion_id",
                table: "tbl_discount_promotion_item");

            migrationBuilder.RenameColumn(
                name: "discount_promotion_id",
                table: "tbl_discount_promotion_item",
                newName: "dicount_promotion_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_discount_promotion_item_discount_promotion_id",
                table: "tbl_discount_promotion_item",
                newName: "IX_tbl_discount_promotion_item_dicount_promotion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_discount_promotion_item_tbl_discount_promotion_dicount_promotion_id",
                table: "tbl_discount_promotion_item",
                column: "dicount_promotion_id",
                principalTable: "tbl_discount_promotion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
