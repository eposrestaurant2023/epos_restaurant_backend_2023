using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class currenct_1dx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "tbl_sale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale",
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
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_note");

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
        }
    }
}
