using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class dddddsd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.RenameColumn(
                name: "PurchaseOrderPaymentModelid",
                table: "tbl_history",
                newName: "purchase_order_payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_history_PurchaseOrderPaymentModelid",
                table: "tbl_history",
                newName: "IX_tbl_history_purchase_order_payment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_purchase_order_payment_id",
                table: "tbl_history",
                column: "purchase_order_payment_id",
                principalTable: "tbl_purchase_order_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_PurchaseOrderPaymentModelid",
                table: "tbl_history");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderPaymentModelid",
                table: "tbl_history",
                newName: "purchase_order_payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_history_PurchaseOrderPaymentModelid",
                table: "tbl_history",
                newName: "IX_tbl_history_purchase_order_payment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_purchase_order_payment_id",
                table: "tbl_history",
                column: "purchase_order_payment_id",
                principalTable: "tbl_purchase_order_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
