using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class change_data_type_datetime_to_datetime2_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_working_day",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale_product_modifier",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale_product",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale_payment",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_expense",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_customer",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_cashier_shift",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_cash_drawer_amount",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "datetime",
                table: "tbl_working_day",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datetime",
                table: "tbl_sale_product_modifier",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datetime",
                table: "tbl_sale_product",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datetime",
                table: "tbl_sale_payment",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datetime",
                table: "tbl_sale",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datetime",
                table: "tbl_expense",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datetime",
                table: "tbl_customer",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datetime",
                table: "tbl_cashier_shift",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datetime",
                table: "tbl_cash_drawer_amount",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }
    }
}
