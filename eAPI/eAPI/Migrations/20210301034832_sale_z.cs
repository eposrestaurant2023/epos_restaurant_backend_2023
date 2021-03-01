using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class sale_z : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreProcedureResults");

            migrationBuilder.DropTable(
                name: "StoreProcedureResultsDecimal");

            migrationBuilder.DropTable(
                name: "tbl_attach_files");

            migrationBuilder.DropTable(
                name: "tbl_business_branch_payment_type");

            migrationBuilder.DropTable(
                name: "tbl_business_branch_price_rule");

            migrationBuilder.DropTable(
                name: "tbl_business_branch_product_price");

            migrationBuilder.DropTable(
                name: "tbl_business_branch_role");

            migrationBuilder.DropTable(
                name: "tbl_business_branch_setting");

            migrationBuilder.DropTable(
                name: "tbl_business_information");

            migrationBuilder.DropTable(
                name: "tbl_config_data");

            migrationBuilder.DropTable(
                name: "tbl_country");

            migrationBuilder.DropTable(
                name: "tbl_customer_business_branch");

            migrationBuilder.DropTable(
                name: "tbl_discount_code");

            migrationBuilder.DropTable(
                name: "tbl_document_number");

            migrationBuilder.DropTable(
                name: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_inventory_transaction");

            migrationBuilder.DropTable(
                name: "tbl_modifier_group_item");

            migrationBuilder.DropTable(
                name: "tbl_modifier_group_product_category");

            migrationBuilder.DropTable(
                name: "tbl_modifier_ingredient");

            migrationBuilder.DropTable(
                name: "tbl_module_view");

            migrationBuilder.DropTable(
                name: "tbl_note");

            migrationBuilder.DropTable(
                name: "tbl_number");

            migrationBuilder.DropTable(
                name: "tbl_outlet_station");

            migrationBuilder.DropTable(
                name: "tbl_permission_option_role");

            migrationBuilder.DropTable(
                name: "tbl_product_ingredient");

            migrationBuilder.DropTable(
                name: "tbl_product_ingredient_related");

            migrationBuilder.DropTable(
                name: "tbl_product_menu");

            migrationBuilder.DropTable(
                name: "tbl_product_modifier");

            migrationBuilder.DropTable(
                name: "tbl_product_price");

            migrationBuilder.DropTable(
                name: "tbl_product_printer");

            migrationBuilder.DropTable(
                name: "tbl_purchase_order_product");

            migrationBuilder.DropTable(
                name: "tbl_sale_product_modifier");

            migrationBuilder.DropTable(
                name: "tbl_sale_product_status");

            migrationBuilder.DropTable(
                name: "tbl_sale_status");

            migrationBuilder.DropTable(
                name: "tbl_shift");

            migrationBuilder.DropTable(
                name: "tbl_station");

            migrationBuilder.DropTable(
                name: "tbl_stock_location_product");

            migrationBuilder.DropTable(
                name: "tbl_stock_take_product");

            migrationBuilder.DropTable(
                name: "tbl_stock_transfer_product");

            migrationBuilder.DropTable(
                name: "tbl_table");

            migrationBuilder.DropTable(
                name: "tbl_setting");

            migrationBuilder.DropTable(
                name: "tbl_payment");

            migrationBuilder.DropTable(
                name: "tbl_purchase_order_payment");

            migrationBuilder.DropTable(
                name: "tbl_inventory_transaction_type");

            migrationBuilder.DropTable(
                name: "tbl_category_note");

            migrationBuilder.DropTable(
                name: "tbl_permission_option");

            migrationBuilder.DropTable(
                name: "tbl_menu");

            migrationBuilder.DropTable(
                name: "tbl_modifier");

            migrationBuilder.DropTable(
                name: "tbl_price_rule");

            migrationBuilder.DropTable(
                name: "tbl_product_portion");

            migrationBuilder.DropTable(
                name: "tbl_printer");

            migrationBuilder.DropTable(
                name: "tbl_sale_product");

            migrationBuilder.DropTable(
                name: "tbl_stock_take");

            migrationBuilder.DropTable(
                name: "tbl_stock_transfer");

            migrationBuilder.DropTable(
                name: "tbl_table_group");

            migrationBuilder.DropTable(
                name: "tbl_payment_type");

            migrationBuilder.DropTable(
                name: "tbl_purchase_order");

            migrationBuilder.DropTable(
                name: "tbl_modifier_group");

            migrationBuilder.DropTable(
                name: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_sale");

            migrationBuilder.DropTable(
                name: "tbl_currency");

            migrationBuilder.DropTable(
                name: "tbl_stock_location");

            migrationBuilder.DropTable(
                name: "tbl_user");

            migrationBuilder.DropTable(
                name: "tbl_kitchen_group");

            migrationBuilder.DropTable(
                name: "tbl_product_category");

            migrationBuilder.DropTable(
                name: "tbl_unit");

            migrationBuilder.DropTable(
                name: "tbl_vendor");

            migrationBuilder.DropTable(
                name: "tbl_customer");

            migrationBuilder.DropTable(
                name: "tbl_outlet");

            migrationBuilder.DropTable(
                name: "tbl_role");

            migrationBuilder.DropTable(
                name: "tbl_product_group");

            migrationBuilder.DropTable(
                name: "tbl_unit_category");

            migrationBuilder.DropTable(
                name: "tbl_province");

            migrationBuilder.DropTable(
                name: "tbl_vendor_group");

            migrationBuilder.DropTable(
                name: "tbl_customer_group");

            migrationBuilder.DropTable(
                name: "tbl_business_branch");
        }
    }
}
