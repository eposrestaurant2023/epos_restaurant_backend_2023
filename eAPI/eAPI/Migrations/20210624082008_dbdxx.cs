using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class dbdxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_category_en",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_category_kh",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_code",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_en",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_kh",
                table: "tbl_product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "product_category_en",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_category_kh",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_group_code",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_group_en",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_group_kh",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN");
        }
    }
}
