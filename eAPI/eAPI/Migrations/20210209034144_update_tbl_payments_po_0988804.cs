using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_payments_po_0988804 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_payment_PaymentModelid",
                table: "tbl_history");

            migrationBuilder.RenameColumn(
                name: "PaymentModelid",
                table: "tbl_history",
                newName: "purchase_order_payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_history_PaymentModelid",
                table: "tbl_history",
                newName: "IX_tbl_history_purchase_order_payment_id");

            migrationBuilder.AddColumn<int>(
                name: "payment_type_id",
                table: "tbl_purchase_order_payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "purhcase_order_id",
                table: "tbl_purchase_order_payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "payment_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_payment_payment_type_id",
                table: "tbl_purchase_order_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_payment_purhcase_order_id",
                table: "tbl_purchase_order_payment",
                column: "purhcase_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_payment_id",
                table: "tbl_history",
                column: "payment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_payment_payment_id",
                table: "tbl_history",
                column: "payment_id",
                principalTable: "tbl_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_purchase_order_payment_id",
                table: "tbl_history",
                column: "purchase_order_payment_id",
                principalTable: "tbl_purchase_order_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_purchase_order_payment_tbl_payment_type_payment_type_id",
                table: "tbl_purchase_order_payment",
                column: "payment_type_id",
                principalTable: "tbl_payment_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_purchase_order_payment_tbl_purchase_order_purhcase_order_id",
                table: "tbl_purchase_order_payment",
                column: "purhcase_order_id",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_payment_payment_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_purchase_order_payment_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_payment_tbl_payment_type_payment_type_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_payment_tbl_purchase_order_purhcase_order_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_payment_payment_type_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_payment_purhcase_order_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_payment_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "payment_type_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "purhcase_order_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "payment_id",
                table: "tbl_history");

            migrationBuilder.RenameColumn(
                name: "purchase_order_payment_id",
                table: "tbl_history",
                newName: "PaymentModelid");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_history_purchase_order_payment_id",
                table: "tbl_history",
                newName: "IX_tbl_history_PaymentModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_payment_PaymentModelid",
                table: "tbl_history",
                column: "PaymentModelid",
                principalTable: "tbl_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
