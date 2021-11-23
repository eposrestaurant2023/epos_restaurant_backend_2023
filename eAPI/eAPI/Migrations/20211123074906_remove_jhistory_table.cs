using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class remove_jhistory_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_customer_customer_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_modifier_group_modifier_group_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_modifier_modifier_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_product_product_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_production_production_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_PurchaseOrderPaymentModelid",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_purchase_order_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_sale_payment_sale_payment_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_sale_sale_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_stock_take_stock_take_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_stock_transfer_stock_transfer_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_user_user_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_vendor_vendor_id",
                table: "tbl_history");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_history",
                table: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_history");

        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_customer_customer_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_modifier_group_modifier_group_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_modifier_modifier_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_product_product_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_production_production_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_purchase_order_payment_PurchaseOrderPaymentModelid",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_purchase_order_purchase_order_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_sale_payment_sale_payment_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_sale_sale_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_stock_take_stock_take_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_stock_transfer_stock_transfer_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_user_user_id",
                table: "HistoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryModel_tbl_vendor_vendor_id",
                table: "HistoryModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoryModel",
                table: "HistoryModel");

            migrationBuilder.RenameTable(
                name: "HistoryModel",
                newName: "tbl_history");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_vendor_id",
                table: "tbl_history",
                newName: "IX_tbl_history_vendor_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_user_id",
                table: "tbl_history",
                newName: "IX_tbl_history_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_stock_transfer_id",
                table: "tbl_history",
                newName: "IX_tbl_history_stock_transfer_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_stock_take_id",
                table: "tbl_history",
                newName: "IX_tbl_history_stock_take_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_sale_payment_id",
                table: "tbl_history",
                newName: "IX_tbl_history_sale_payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_sale_id",
                table: "tbl_history",
                newName: "IX_tbl_history_sale_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_PurchaseOrderPaymentModelid",
                table: "tbl_history",
                newName: "IX_tbl_history_PurchaseOrderPaymentModelid");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_purchase_order_id",
                table: "tbl_history",
                newName: "IX_tbl_history_purchase_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_production_id",
                table: "tbl_history",
                newName: "IX_tbl_history_production_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_product_id",
                table: "tbl_history",
                newName: "IX_tbl_history_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_modifier_id",
                table: "tbl_history",
                newName: "IX_tbl_history_modifier_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_modifier_group_id",
                table: "tbl_history",
                newName: "IX_tbl_history_modifier_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryModel_customer_id",
                table: "tbl_history",
                newName: "IX_tbl_history_customer_id");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "tbl_history",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_history",
                table: "tbl_history",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_customer_customer_id",
                table: "tbl_history",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_modifier_group_modifier_group_id",
                table: "tbl_history",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_modifier_modifier_id",
                table: "tbl_history",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_product_product_id",
                table: "tbl_history",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_production_production_id",
                table: "tbl_history",
                column: "production_id",
                principalTable: "tbl_production",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_PurchaseOrderPaymentModelid",
                table: "tbl_history",
                column: "PurchaseOrderPaymentModelid",
                principalTable: "tbl_purchase_order_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_purchase_order_id",
                table: "tbl_history",
                column: "purchase_order_id",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_sale_payment_sale_payment_id",
                table: "tbl_history",
                column: "sale_payment_id",
                principalTable: "tbl_sale_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_sale_sale_id",
                table: "tbl_history",
                column: "sale_id",
                principalTable: "tbl_sale",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_stock_take_stock_take_id",
                table: "tbl_history",
                column: "stock_take_id",
                principalTable: "tbl_stock_take",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_stock_transfer_stock_transfer_id",
                table: "tbl_history",
                column: "stock_transfer_id",
                principalTable: "tbl_stock_transfer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_user_user_id",
                table: "tbl_history",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_vendor_vendor_id",
                table: "tbl_history",
                column: "vendor_id",
                principalTable: "tbl_vendor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
