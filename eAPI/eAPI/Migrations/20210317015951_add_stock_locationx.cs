using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_stock_locationx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "stock_location_id",
                table: "tbl_purchase_order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "stock_location_id",
                table: "tbl_inventory_transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_stock_location_id",
                table: "tbl_purchase_order",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_stock_location_id",
                table: "tbl_inventory_transaction",
                column: "stock_location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_stock_location_stock_location_id",
                table: "tbl_inventory_transaction",
                column: "stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_purchase_order_tbl_stock_location_stock_location_id",
                table: "tbl_purchase_order",
                column: "stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_stock_location_stock_location_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_tbl_stock_location_stock_location_id",
                table: "tbl_purchase_order");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_stock_location_id",
                table: "tbl_purchase_order");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_transaction_stock_location_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "stock_location_id",
                table: "tbl_purchase_order");

            migrationBuilder.DropColumn(
                name: "stock_location_id",
                table: "tbl_inventory_transaction");
        }
    }
}
