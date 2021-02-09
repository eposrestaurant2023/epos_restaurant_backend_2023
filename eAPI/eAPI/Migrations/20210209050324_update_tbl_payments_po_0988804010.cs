using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_payments_po_0988804010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_payment_tbl_purchase_order_purhcase_order_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_payment_purhcase_order_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "purhcase_order_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.AddColumn<int>(
                name: "purchase_order_id",
                table: "tbl_purchase_order_payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_payment_purchase_order_id",
                table: "tbl_purchase_order_payment",
                column: "purchase_order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_purchase_order_payment_tbl_purchase_order_purchase_order_id",
                table: "tbl_purchase_order_payment",
                column: "purchase_order_id",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_payment_tbl_purchase_order_purchase_order_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_payment_purchase_order_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "purchase_order_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.AddColumn<int>(
                name: "purhcase_order_id",
                table: "tbl_purchase_order_payment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_payment_purhcase_order_id",
                table: "tbl_purchase_order_payment",
                column: "purhcase_order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_purchase_order_payment_tbl_purchase_order_purhcase_order_id",
                table: "tbl_purchase_order_payment",
                column: "purhcase_order_id",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
