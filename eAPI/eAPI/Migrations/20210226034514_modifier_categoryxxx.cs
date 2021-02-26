﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class modifier_categoryxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "parentid",
                table: "tbl_product_modifier",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_parentid",
                table: "tbl_product_modifier",
                column: "parentid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_parentid",
                table: "tbl_product_modifier",
                column: "parentid",
                principalTable: "tbl_product_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_parentid",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_parentid",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "parentid",
                table: "tbl_product_modifier");
        }
    }
}
