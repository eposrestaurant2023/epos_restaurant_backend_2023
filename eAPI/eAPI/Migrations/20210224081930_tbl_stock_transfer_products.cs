﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class tbl_stock_transfer_products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_product_tbl_unit_unit_id",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_product_unit_id",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropColumn(
                name: "discount",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropColumn(
                name: "invoice_discount_amount",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropColumn(
                name: "discount",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "discount_type",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "discountable_amount",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "grand_total_discount",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "stock_take_product_discount_amount",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "total_discount",
                table: "tbl_stock_transfer");

            migrationBuilder.RenameColumn(
                name: "total_discount",
                table: "tbl_stock_transfer_product",
                newName: "regular_cost");

            migrationBuilder.RenameColumn(
                name: "discount_type",
                table: "tbl_stock_transfer_product",
                newName: "unit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "unit",
                table: "tbl_stock_transfer_product",
                newName: "discount_type");

            migrationBuilder.RenameColumn(
                name: "regular_cost",
                table: "tbl_stock_transfer_product",
                newName: "total_discount");

            migrationBuilder.AddColumn<decimal>(
                name: "discount",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_stock_transfer_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_stock_transfer_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "discount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "discount_type",
                table: "tbl_stock_transfer",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "discountable_amount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "stock_take_product_discount_amount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_discount",
                table: "tbl_stock_transfer",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_product_unit_id",
                table: "tbl_stock_transfer_product",
                column: "unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_product_tbl_unit_unit_id",
                table: "tbl_stock_transfer_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
