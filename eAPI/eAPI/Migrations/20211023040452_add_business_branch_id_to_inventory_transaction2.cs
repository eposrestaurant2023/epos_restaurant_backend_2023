using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_business_branch_id_to_inventory_transaction2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_working_day",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_working_day",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_vendor_group",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_vendor_group",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_vendor",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_vendor",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_user",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_user",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_unit",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_unit",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_table_group",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_table_group",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_table",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_table",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_stock_transfer_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_stock_transfer_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_stock_transfer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_stock_transfer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_stock_take_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_stock_take_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_stock_take",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_stock_take",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_station",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_station",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_sale_product_modifier",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale_product_modifier",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_sale_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_sale_payment",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale_payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_role",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_role",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_purchase_order_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_purchase_order_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_purchase_order_payment",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_purchase_order_payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_purchase_order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_purchase_order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_product_tax",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_product_tax",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_product_price",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_product_price",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_product_portion",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_product_portion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_product_modifier",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_product_modifier",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_product_ingredient",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_product_ingredient",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_product_group",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_product_group",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_product_category",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_product_category",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_printer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_printer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_price_rule",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_price_rule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_payment_type",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_payment_type",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_outlet",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_outlet",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_note",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_note",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_modifier_group_item",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_modifier_group_item",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_modifier_group",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_modifier_group",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_modifier",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_modifier",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_menu",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_history",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_history",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_discount_code",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_discount_code",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_customer_group",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_customer_group",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_customer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_cashier_shift",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_cashier_shift",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_business_branch",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_business_branch",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_attach_files",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_attach_files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_vendor_group");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_vendor_group");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_vendor");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_vendor");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_user");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_user");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_unit");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_unit");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_table_group");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_table_group");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_table");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_table");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_stock_take_product");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_stock_take_product");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_stock_take");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_stock_take");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_role");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_role");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_purchase_order");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_purchase_order");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_product_tax");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_product_tax");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_product_price");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_product_price");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_product_portion");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_product_portion");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_product_ingredient");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_product_ingredient");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_product_group");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_product_group");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_product_category");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_product_category");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_printer");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_printer");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_price_rule");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_price_rule");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_outlet");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_outlet");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_modifier_group");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_modifier_group");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_modifier");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_modifier");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_menu");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_menu");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_discount_code");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_discount_code");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_business_branch");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_business_branch");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_attach_files");

            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_inventory_transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
