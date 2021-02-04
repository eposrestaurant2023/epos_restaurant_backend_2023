using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class hhh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "max_price",
                table: "tbl_product",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "min_price",
                table: "tbl_product",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "max_price",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "min_price",
                table: "tbl_product");
        }
    }
}
