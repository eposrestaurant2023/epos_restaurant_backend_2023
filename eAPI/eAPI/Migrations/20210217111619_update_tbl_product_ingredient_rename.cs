using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_ingredient_rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_ingredient_tbl_product_product_ingredient_id",
                table: "tbl_product_ingredient");

            migrationBuilder.RenameColumn(
                name: "product_ingredient_id",
                table: "tbl_product_ingredient",
                newName: "ingredient_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_product_ingredient_product_ingredient_id",
                table: "tbl_product_ingredient",
                newName: "IX_tbl_product_ingredient_ingredient_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "tbl_config_data",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_ingredient_tbl_product_ingredient_id",
                table: "tbl_product_ingredient",
                column: "ingredient_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_ingredient_tbl_product_ingredient_id",
                table: "tbl_product_ingredient");

            migrationBuilder.RenameColumn(
                name: "ingredient_id",
                table: "tbl_product_ingredient",
                newName: "product_ingredient_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_product_ingredient_ingredient_id",
                table: "tbl_product_ingredient",
                newName: "IX_tbl_product_ingredient_product_ingredient_id");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "tbl_config_data",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_ingredient_tbl_product_product_ingredient_id",
                table: "tbl_product_ingredient",
                column: "product_ingredient_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
