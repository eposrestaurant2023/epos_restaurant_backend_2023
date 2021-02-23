using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_stock_take_09078 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sub_total",
                table: "tbl_stock_take");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
