using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_ingredient_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "tbl_product_ingredient",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "quantity",
                table: "tbl_product_ingredient",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_cost",
                table: "tbl_product_ingredient",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_product_ingredient",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost",
                table: "tbl_product_ingredient");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "tbl_product_ingredient");

            migrationBuilder.DropColumn(
                name: "total_cost",
                table: "tbl_product_ingredient");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_product_ingredient");
        }
    }
}
