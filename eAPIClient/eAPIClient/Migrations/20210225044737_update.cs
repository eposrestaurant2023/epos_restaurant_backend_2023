using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_sale_product");

            migrationBuilder.RenameColumn(
                name: "unit",
                table: "tbl_sale_product",
                newName: "product_name_kh");

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
                name: "keyword",
                table: "tbl_product",
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
                name: "keyword",
                table: "tbl_product");

            migrationBuilder.RenameColumn(
                name: "product_name_kh",
                table: "tbl_sale_product",
                newName: "unit");

            migrationBuilder.AddColumn<decimal>(
                name: "multiplier",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
