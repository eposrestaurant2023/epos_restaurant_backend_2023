using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class dddd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_product_ProductModelid",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_PurchaseOrderModelid",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_ProductModelid",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_PurchaseOrderModelid",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "ProductModelid",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderModelid",
                table: "tbl_history");
 

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_product_id",
                table: "tbl_history",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_purchase_order_id",
                table: "tbl_history",
                column: "purchase_order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_product_product_id",
                table: "tbl_history",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_purchase_order_id",
                table: "tbl_history",
                column: "purchase_order_id",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_product_product_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_purchase_order_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_product_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_purchase_order_id",
                table: "tbl_history");

            migrationBuilder.AddColumn<int>(
                name: "ProductModelid",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderModelid",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "tbl_business_information",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_ProductModelid",
                table: "tbl_history",
                column: "ProductModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_PurchaseOrderModelid",
                table: "tbl_history",
                column: "PurchaseOrderModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_product_ProductModelid",
                table: "tbl_history",
                column: "ProductModelid",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_PurchaseOrderModelid",
                table: "tbl_history",
                column: "PurchaseOrderModelid",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
