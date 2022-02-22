using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_inventory_checkj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "user_code",
                table: "tbl_user",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "tbl_user",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_inventory_date",
                table: "tbl_inventory_check_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_check_product_product_id",
                table: "tbl_inventory_check_product",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_check_product_tbl_product_product_id",
                table: "tbl_inventory_check_product",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_check_product_tbl_product_product_id",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_check_product_product_id",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "last_inventory_date",
                table: "tbl_inventory_check_product");

            migrationBuilder.AlterColumn<string>(
                name: "user_code",
                table: "tbl_user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "tbl_user",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");
        }
    }
}
