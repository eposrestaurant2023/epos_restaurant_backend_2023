using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class Add_redeem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_park",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_redeem_park",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "park_expired_date",
                table: "tbl_sale_product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "park_note",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "park_sale_product_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "redeem_sale_product_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_park",
                table: "tbl_sale",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_redeem_park",
                table: "tbl_sale",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "park_sale_id",
                table: "tbl_sale",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "redeem_park_sale_id",
                table: "tbl_sale",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_park",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "is_redeem_park",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "park_expired_date",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "park_note",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "park_sale_product_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "redeem_sale_product_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "is_park",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "is_redeem_park",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "park_sale_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "redeem_park_sale_id",
                table: "tbl_sale");
        }
    }
}
