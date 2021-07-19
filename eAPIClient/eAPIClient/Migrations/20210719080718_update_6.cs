using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class update_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "product_modifier_id",
                table: "tbl_sale_product_modifier",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "SaleProductModelid",
                table: "tbl_sale_product_modifier",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_modifier_SaleProductModelid",
                table: "tbl_sale_product_modifier",
                column: "SaleProductModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_modifier_tbl_sale_product_SaleProductModelid",
                table: "tbl_sale_product_modifier",
                column: "SaleProductModelid",
                principalTable: "tbl_sale_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
