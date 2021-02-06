using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_price_rulexxxxxxxxxs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_product_price_tbl_product_portion_ProductPortionModelid",
                table: "tbl_business_branch_product_price");

            migrationBuilder.DropIndex(
                name: "IX_tbl_business_branch_product_price_ProductPortionModelid",
                table: "tbl_business_branch_product_price");

            migrationBuilder.DropColumn(
                name: "ProductPortionModelid",
                table: "tbl_business_branch_product_price");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductPortionModelid",
                table: "tbl_business_branch_product_price",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_product_price_ProductPortionModelid",
                table: "tbl_business_branch_product_price",
                column: "ProductPortionModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_product_price_tbl_product_portion_ProductPortionModelid",
                table: "tbl_business_branch_product_price",
                column: "ProductPortionModelid",
                principalTable: "tbl_product_portion",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
