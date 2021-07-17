using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class @decimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "total_payable",
                table: "tbl_vendor",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "weight",
                table: "tbl_unit_category",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "volumes",
                table: "tbl_unit_category",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "length",
                table: "tbl_unit_category",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_unit",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_cost",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_stock_transfer",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_transfer",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_transfer",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_take_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_take_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_cost",
                table: "tbl_stock_take_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_take_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_take_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_stock_take_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_stock_take_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_stock_take",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_take",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_location_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "min_quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "max_quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "initial_quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "initial_adjustment_quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_taxable_rate",
                table: "tbl_station",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_station",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_taxable_rate",
                table: "tbl_station",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_station",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_taxable_rate",
                table: "tbl_station",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_station",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "reqular_price",
                table: "tbl_sale_product_modifier",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_sale_product_modifier",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_tax_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_modifier_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_taxable_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_taxable_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_taxable_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_product_discount_value",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_product_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_discount_value",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "reqular_price",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "profit",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "net_sale",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_sale_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "payment_amount",
                table: "tbl_sale_payment",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "change_amount",
                table: "tbl_sale_payment",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_tax_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_profit",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_net_sale",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_credit",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_product_discount_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_discount_value",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_discount_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "paid_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_cost",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_purchase_order_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "payment_amount",
                table: "tbl_purchase_order_payment",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "po_product_discount_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "paid_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "tbl_purchase_order",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_product_tax",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_product_tax",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_product_tax",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_price",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_product_portion",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product_portion",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_modifier",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost",
                table: "tbl_product_ingredient",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_product_ingredient",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product_ingredient",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "min_price",
                table: "tbl_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "max_price",
                table: "tbl_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "predefine_value",
                table: "tbl_predefine_payment_amount",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount_value",
                table: "tbl_predefine_discount_code",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost",
                table: "tbl_modifier_ingredient",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_modifier_ingredient",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_modifier_ingredient",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_modifier_group_item",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity_on_hand",
                table: "tbl_inventory_transaction",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_inventory_transaction",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "old_quantity",
                table: "tbl_inventory_transaction",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_inventory_transaction",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "old_amount",
                table: "tbl_history",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "tbl_history",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount_value",
                table: "tbl_discount_code",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_receivable",
                table: "tbl_customer",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "open_amount",
                table: "tbl_cashier_shift",
                type: "decimal(19,8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "exchange_rate",
                table: "tbl_cashier_shift",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "close_amount",
                table: "tbl_cashier_shift",
                type: "decimal(19,8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "result",
                table: "StoreProcedureResultsDecimal",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "total_payable",
                table: "tbl_vendor",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "weight",
                table: "tbl_unit_category",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "volumes",
                table: "tbl_unit_category",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "length",
                table: "tbl_unit_category",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_unit",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_cost",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_cost",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_stock_take_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_stock_take",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_stock_location_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "min_quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "max_quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "initial_quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "initial_adjustment_quantity",
                table: "tbl_stock_location_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_taxable_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_taxable_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_taxable_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "reqular_price",
                table: "tbl_sale_product_modifier",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_sale_product_modifier",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_tax_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_modifier_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_taxable_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_taxable_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_taxable_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_product_discount_value",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_product_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_discount_value",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "reqular_price",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "profit",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "net_sale",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "payment_amount",
                table: "tbl_sale_payment",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "change_amount",
                table: "tbl_sale_payment",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_tax_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_profit",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_net_sale",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_credit",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_product_discount_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_discount_value",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_discount_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "paid_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "regular_cost",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_purchase_order_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "payment_amount",
                table: "tbl_purchase_order_payment",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_quantity",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_discount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_total",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "po_product_discount_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "paid_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "tbl_purchase_order",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_product_tax",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_product_tax",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_product_tax",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_price",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_product_portion",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product_portion",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_modifier",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost",
                table: "tbl_product_ingredient",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_product_ingredient",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product_ingredient",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "min_price",
                table: "tbl_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "max_price",
                table: "tbl_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "predefine_value",
                table: "tbl_predefine_payment_amount",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount_value",
                table: "tbl_predefine_discount_code",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost",
                table: "tbl_modifier_ingredient",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_modifier_ingredient",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_modifier_ingredient",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_modifier_group_item",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity_on_hand",
                table: "tbl_inventory_transaction",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_inventory_transaction",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "old_quantity",
                table: "tbl_inventory_transaction",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_inventory_transaction",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "old_amount",
                table: "tbl_history",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "tbl_history",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount_value",
                table: "tbl_discount_code",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "total_receivable",
                table: "tbl_customer",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "open_amount",
                table: "tbl_cashier_shift",
                type: "decimal(19,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "exchange_rate",
                table: "tbl_cashier_shift",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "close_amount",
                table: "tbl_cashier_shift",
                type: "decimal(19,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "result",
                table: "StoreProcedureResultsDecimal",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");
        }
    }
}
