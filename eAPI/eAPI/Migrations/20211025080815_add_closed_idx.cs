using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_closed_idx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
   
             
  
     

             

            migrationBuilder.AddColumn<Guid>(
                name: "cash_drawer_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "closed_cash_drawer_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "closed_cashier_shift_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "closed_outlet_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "closed_station_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "closed_working_day_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_cash_drawer");

            migrationBuilder.DropTable(
                name: "tbl_cash_drawer_amount");

            migrationBuilder.DropColumn(
                name: "cash_drawer_id",
                table: "tbl_working_day");

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
                name: "cash_drawer_id",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "cash_drawer_name",
                table: "tbl_station");

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
                name: "currency_format",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "cash_drawer_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_cash_drawer_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_cashier_shift_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_station_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_working_day_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "deleted_note",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "is_foc",
                table: "tbl_sale");

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
                name: "currency_format",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "currency_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "currency_name_en",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "currency_name_kh",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "exchange_rate",
                table: "tbl_purchase_order_payment");

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
                name: "payment_type_group",
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "last_modified_by",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");
        }
    }
}
