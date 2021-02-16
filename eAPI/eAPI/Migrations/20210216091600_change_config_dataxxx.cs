using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_config_dataxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "total_payable",
                table: "tbl_vendor",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "stock_take_product_discount_amount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "stock_take_product_discount_amount",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "selling_price",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_price",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_product_discount_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "paid_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "selling_price",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_price",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "payment_amount",
                table: "tbl_purchase_order_payment",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "po_product_discount_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "paid_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_price",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_product_portion",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product_portion",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_modifier",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "min_price",
                table: "tbl_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "max_price",
                table: "tbl_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "payment_amount",
                table: "tbl_payment",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "old_amount",
                table: "tbl_history",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "tbl_history",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount_value",
                table: "tbl_discount_code",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "exchange_rate",
                table: "tbl_currency",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "change_exchange_rate",
                table: "tbl_currency",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "result",
                table: "StoreProcedureResultsDecimal",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "total_payable",
                table: "tbl_vendor",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_stock_transfer_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_transfer_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_transfer_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_transfer_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_transfer_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_stock_transfer_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_stock_transfer_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_stock_transfer_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_stock_transfer_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_stock_transfer",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_stock_transfer",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_transfer",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_transfer",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "stock_take_product_discount_amount",
                table: "tbl_stock_transfer",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_stock_transfer",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_stock_transfer",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_stock_transfer",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_stock_take_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_take_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_take_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_take_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_take_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_stock_take_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_stock_take_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_stock_take_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_stock_take_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_stock_take",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_stock_take",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_take",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_take",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "stock_take_product_discount_amount",
                table: "tbl_stock_take",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_stock_take",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_stock_take",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_stock_take",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "selling_price",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_price",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_sale_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_product_discount_amount",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "paid_amount",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "tbl_sale",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "selling_price",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_price",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_purchase_order_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "payment_amount",
                table: "tbl_purchase_order_payment",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "po_product_discount_amount",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "paid_amount",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "tbl_purchase_order",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_price",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_product_portion",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product_portion",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_modifier",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "min_price",
                table: "tbl_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "max_price",
                table: "tbl_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "payment_amount",
                table: "tbl_payment",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "old_amount",
                table: "tbl_history",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "tbl_history",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount_value",
                table: "tbl_discount_code",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "exchange_rate",
                table: "tbl_currency",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "change_exchange_rate",
                table: "tbl_currency",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "result",
                table: "StoreProcedureResultsDecimal",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");
        }
    }
}
