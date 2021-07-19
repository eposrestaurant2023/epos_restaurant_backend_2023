using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class update_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_modifier_tbl_sale_product_sale_product_id",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_product_modifier_sale_product_id",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropColumn(
                name: "product_modifier_id",
                table: "tbl_sale_product_modifier");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_modifier_tbl_sale_product_SaleProductModelid",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_product_modifier_SaleProductModelid",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropColumn(
                name: "SaleProductModelid",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropColumn(
                name: "is_open_product",
                table: "tbl_product");

            migrationBuilder.AddColumn<int>(
                name: "product_modifier_id",
                table: "tbl_sale_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_modifier_sale_product_id",
                table: "tbl_sale_product_modifier",
                column: "sale_product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_modifier_tbl_sale_product_sale_product_id",
                table: "tbl_sale_product_modifier",
                column: "sale_product_id",
                principalTable: "tbl_sale_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
