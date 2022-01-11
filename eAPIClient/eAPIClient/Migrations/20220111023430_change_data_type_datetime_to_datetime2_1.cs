using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class change_data_type_datetime_to_datetime2_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_working_day",
                newName: "datetime");

            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_sale_product_modifier",
                newName: "datetime");

            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_sale_product",
                newName: "datetime");

            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_sale_payment",
                newName: "datetime");

            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_sale",
                newName: "datetime");

            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_expense",
                newName: "datetime");

            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_customer",
                newName: "datetime");

            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_cashier_shift",
                newName: "datetime");

            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_cash_drawer_amount",
                newName: "datetime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "tbl_working_day",
                newName: "last_modified_date");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "tbl_sale_product_modifier",
                newName: "last_modified_date");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "tbl_sale_product",
                newName: "last_modified_date");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "tbl_sale_payment",
                newName: "last_modified_date");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "tbl_sale",
                newName: "last_modified_date");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "tbl_expense",
                newName: "last_modified_date");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "tbl_customer",
                newName: "last_modified_date");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "tbl_cashier_shift",
                newName: "last_modified_date");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "tbl_cash_drawer_amount",
                newName: "last_modified_date");
        }
    }
}
