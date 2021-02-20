﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_unit_category_id12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_unit_category_id",
                table: "tbl_product",
                column: "unit_category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_unit_category_unit_category_id",
                table: "tbl_product",
                column: "unit_category_id",
                principalTable: "tbl_unit_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_unit_category_unit_category_id",
                table: "tbl_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_unit_category_id",
                table: "tbl_product");
        }
    }
}
