using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_invtoryxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_product_tbl_unit_unit_id",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_product_unit_id",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_purchase_order_product");

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_purchase_order_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "multiplier",
                table: "tbl_inventory_transaction",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "tbl_inventory_transaction",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "purchase_order_product_id",
                table: "tbl_inventory_transaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_inventory_transaction",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "note",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "purchase_order_product_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_inventory_transaction");

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_purchase_order_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_product_unit_id",
                table: "tbl_purchase_order_product",
                column: "unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_purchase_order_product_tbl_unit_unit_id",
                table: "tbl_purchase_order_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
