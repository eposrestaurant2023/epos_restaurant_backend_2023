using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_po : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sale_product_discount_amount",
                table: "tbl_purchase_order",
                newName: "po_product_discount_amount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "po_product_discount_amount",
                table: "tbl_purchase_order",
                newName: "sale_product_discount_amount");
        }
    }
}
