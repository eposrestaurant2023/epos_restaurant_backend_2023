using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class update9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_product_modifier",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "ProductModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.AddColumn<Guid>(
                name: "id_test",
                table: "tbl_product_modifier",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductModifierModelid_test",
                table: "tbl_product_modifier",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_product_modifier",
                table: "tbl_product_modifier",
                column: "id_test");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_ProductModifierModelid_test",
                table: "tbl_product_modifier",
                column: "ProductModifierModelid_test");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_ProductModifierModelid_test",
                table: "tbl_product_modifier",
                column: "ProductModifierModelid_test",
                principalTable: "tbl_product_modifier",
                principalColumn: "id_test",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_ProductModifierModelid_test",
                table: "tbl_product_modifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_product_modifier",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_ProductModifierModelid_test",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "id_test",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "ProductModifierModelid_test",
                table: "tbl_product_modifier");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductModifierModelid",
                table: "tbl_product_modifier",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_product_modifier",
                table: "tbl_product_modifier",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier",
                column: "ProductModifierModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier",
                column: "ProductModifierModelid",
                principalTable: "tbl_product_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
