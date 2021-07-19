using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_modifer_guid1xxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_product_modifier");

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_id",
                table: "tbl_product_modifier",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_modifier_id",
                table: "tbl_product_modifier",
                column: "modifier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_modifier_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "modifier_id",
                table: "tbl_product_modifier");

            migrationBuilder.AddColumn<int>(
                name: "modifier_group_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
