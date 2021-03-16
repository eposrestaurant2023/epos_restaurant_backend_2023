using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class ssxx33xxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
  

            migrationBuilder.AddColumn<bool>(
                name: "is_build_in",
                table: "tbl_price_rule",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_build_in",
                table: "tbl_price_rule");

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseOrderPaymentModelid",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_PurchaseOrderPaymentModelid",
                table: "tbl_history",
                column: "PurchaseOrderPaymentModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_PurchaseOrderPaymentModelid",
                table: "tbl_history",
                column: "PurchaseOrderPaymentModelid",
                principalTable: "tbl_purchase_order_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
