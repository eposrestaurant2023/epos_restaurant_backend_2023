using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_invtoryxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "regular_price",
                table: "tbl_purchase_order_product");

            migrationBuilder.RenameColumn(
                name: "selling_price",
                table: "tbl_purchase_order_product",
                newName: "regular_cost");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "regular_cost",
                table: "tbl_purchase_order_product",
                newName: "selling_price");

            migrationBuilder.AddColumn<decimal>(
                name: "regular_price",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
