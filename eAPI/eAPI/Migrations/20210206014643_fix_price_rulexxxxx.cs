using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_price_rulexxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_product_price_tbl_product_portion_portion_id",
                table: "tbl_business_branch_product_price");

            migrationBuilder.RenameColumn(
                name: "portion_id",
                table: "tbl_business_branch_product_price",
                newName: "product_portion_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_business_branch_product_price_portion_id",
                table: "tbl_business_branch_product_price",
                newName: "IX_tbl_business_branch_product_price_product_portion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_product_price_tbl_product_portion_product_portion_id",
                table: "tbl_business_branch_product_price",
                column: "product_portion_id",
                principalTable: "tbl_product_portion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_product_price_tbl_product_portion_product_portion_id",
                table: "tbl_business_branch_product_price");

            migrationBuilder.RenameColumn(
                name: "product_portion_id",
                table: "tbl_business_branch_product_price",
                newName: "portion_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_business_branch_product_price_product_portion_id",
                table: "tbl_business_branch_product_price",
                newName: "IX_tbl_business_branch_product_price_portion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_product_price_tbl_product_portion_portion_id",
                table: "tbl_business_branch_product_price",
                column: "portion_id",
                principalTable: "tbl_product_portion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
