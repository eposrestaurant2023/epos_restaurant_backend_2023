using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class tbl_product_category_009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_product_category",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_product_category",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_product_category",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tax_1_rate",
                table: "tbl_product_category");

            migrationBuilder.DropColumn(
                name: "tax_2_rate",
                table: "tbl_product_category");

            migrationBuilder.DropColumn(
                name: "tax_3_rate",
                table: "tbl_product_category");
        }
    }
}
