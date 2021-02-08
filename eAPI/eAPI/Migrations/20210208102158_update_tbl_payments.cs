using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_payments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_tbl_purchase_order_PurchaseOrderModelid",
                table: "tbl_payment");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderModelid",
                table: "tbl_payment",
                newName: "purchase_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_payment_PurchaseOrderModelid",
                table: "tbl_payment",
                newName: "IX_tbl_payment_purchase_order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_tbl_purchase_order_purchase_order_id",
                table: "tbl_payment",
                column: "purchase_order_id",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_tbl_purchase_order_purchase_order_id",
                table: "tbl_payment");

            migrationBuilder.RenameColumn(
                name: "purchase_order_id",
                table: "tbl_payment",
                newName: "PurchaseOrderModelid");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_payment_purchase_order_id",
                table: "tbl_payment",
                newName: "IX_tbl_payment_PurchaseOrderModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_tbl_purchase_order_PurchaseOrderModelid",
                table: "tbl_payment",
                column: "PurchaseOrderModelid",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
