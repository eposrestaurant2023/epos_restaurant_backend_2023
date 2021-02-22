using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_invtoryx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_low_inventory",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_out_of_stock",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_over_stock",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "quantity",
                table: "tbl_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_low_inventory",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "is_out_of_stock",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "is_over_stock",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "tbl_product");
        }
    }
}
