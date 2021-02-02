using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_dbxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost",
                table: "tbl_product_price");

            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "tbl_product_portion",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "tbl_price_rule",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost",
                table: "tbl_product_portion");

            migrationBuilder.DropColumn(
                name: "is_default",
                table: "tbl_price_rule");

            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "tbl_product_price",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
