using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class last_modifie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "tbl_sale",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "tbl_sale");

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
        }
    }
}
