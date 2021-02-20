using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbls_unit_mutiplier_90989 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_tbl_unit_unit_id",
                table: "tbl_purchase_order");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_take_tbl_unit_unit_id",
                table: "tbl_stock_take");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_tbl_unit_unit_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_unit_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_take_unit_id",
                table: "tbl_stock_take");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_unit_id",
                table: "tbl_purchase_order");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_stock_take_product");

            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_stock_take");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_stock_take");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_purchase_order");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_purchase_order");

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_stock_transfer_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_stock_take_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_purchase_order_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_product_unit_id",
                table: "tbl_stock_transfer_product",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_take_product_unit_id",
                table: "tbl_stock_take_product",
                column: "unit_id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_take_product_tbl_unit_unit_id",
                table: "tbl_stock_take_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_product_tbl_unit_unit_id",
                table: "tbl_stock_transfer_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_product_tbl_unit_unit_id",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_take_product_tbl_unit_unit_id",
                table: "tbl_stock_take_product");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_product_tbl_unit_unit_id",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_product_unit_id",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_take_product_unit_id",
                table: "tbl_stock_take_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_product_unit_id",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_stock_take_product");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_purchase_order_product");

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_stock_transfer_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_stock_transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_stock_take_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_stock_take",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_purchase_order_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "multiplier",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_purchase_order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_unit_id",
                table: "tbl_stock_transfer",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_take_unit_id",
                table: "tbl_stock_take",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_unit_id",
                table: "tbl_purchase_order",
                column: "unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_purchase_order_tbl_unit_unit_id",
                table: "tbl_purchase_order",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_take_tbl_unit_unit_id",
                table: "tbl_stock_take",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_tbl_unit_unit_id",
                table: "tbl_stock_transfer",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
