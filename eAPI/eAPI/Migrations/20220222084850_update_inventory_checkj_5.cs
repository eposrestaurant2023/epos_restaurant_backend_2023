using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_inventory_checkj_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "product_category_name",
                table: "tbl_inventory_check_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_group_name",
                table: "tbl_inventory_check_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_name",
                table: "tbl_inventory_check_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "unit_name",
                table: "tbl_inventory_check_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_category_name",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "product_group_name",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "product_name",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "unit_name",
                table: "tbl_inventory_check_product");
        }
    }
}
