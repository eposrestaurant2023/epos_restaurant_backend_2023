using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class ADD_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "product_code",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_name_en",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_name_kh",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_code",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "product_name_en",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "product_name_kh",
                table: "tbl_sale_product");
        }
    }
}
