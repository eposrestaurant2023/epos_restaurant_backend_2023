using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class is_shortcut_mx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_sale_product_print_queue",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_sale_product_print_queue",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                table: "tbl_sale_product_print_queue",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_sale_product_print_queue",
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
        }
    }
}
