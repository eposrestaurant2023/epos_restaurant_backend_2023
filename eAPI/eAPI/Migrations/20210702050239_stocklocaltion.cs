﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class stocklocaltion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_tbl_stock_location_stock_location_id",
                table: "tbl_sale_product");

            migrationBuilder.AlterColumn<Guid>(
                name: "stock_location_id",
                table: "tbl_sale_product",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_tbl_stock_location_stock_location_id",
                table: "tbl_sale_product",
                column: "stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
