using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_modf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_cashier_shift");

            migrationBuilder.RenameColumn(
                name: "last_modified",
                table: "tbl_sale",
                newName: "last_modified_date");

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "last_modified_date",
                table: "tbl_sale",
                newName: "last_modified");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_working_day",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_sale_product_modifier",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_sale_product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_sale_payment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_note",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_customer_group",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_cashier_shift",
                type: "datetime2",
                nullable: true);
        }
    }
}
