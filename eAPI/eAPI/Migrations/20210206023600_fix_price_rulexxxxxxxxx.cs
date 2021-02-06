using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_price_rulexxxxxxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_product_price_tbl_product_portion_product_portion_id",
                table: "tbl_business_branch_product_price");

            migrationBuilder.DropIndex(
                name: "IX_tbl_business_branch_product_price_product_portion_id",
                table: "tbl_business_branch_product_price");

            migrationBuilder.DropColumn(
                name: "is_default",
                table: "tbl_business_branch_product_price");

            migrationBuilder.DropColumn(
                name: "price",
                table: "tbl_business_branch_product_price");

            migrationBuilder.DropColumn(
                name: "price_rule_id",
                table: "tbl_business_branch_product_price");

            migrationBuilder.AddColumn<int>(
                name: "ProductPortionModelid",
                table: "tbl_business_branch_product_price",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "prices",
                table: "tbl_business_branch_product_price",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "prices",
                table: "tbl_business_branch_product_price");

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "tbl_business_branch_product_price",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "tbl_business_branch_product_price",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "price_rule_id",
                table: "tbl_business_branch_product_price",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_product_price_product_portion_id",
                table: "tbl_business_branch_product_price",
                column: "product_portion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_product_price_tbl_product_portion_product_portion_id",
                table: "tbl_business_branch_product_price",
                column: "product_portion_id",
                principalTable: "tbl_product_portion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
