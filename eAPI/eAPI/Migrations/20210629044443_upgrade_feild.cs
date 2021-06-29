using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class upgrade_feild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_free",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "product_category_en",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "product_category_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "product_category_kh",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_group_en",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "product_group_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "product_group_kh",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_free",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "product_category_en",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "product_category_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "product_category_kh",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "product_group_en",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "product_group_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "product_group_kh",
                table: "tbl_sale_product");
        }
    }
}
