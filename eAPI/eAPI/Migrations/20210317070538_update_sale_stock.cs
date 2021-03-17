using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_sale_stock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "stock_location_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "menu_name_kh",
                table: "tbl_menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_stock_location_id",
                table: "tbl_sale",
                column: "stock_location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_stock_location_stock_location_id",
                table: "tbl_sale",
                column: "stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_stock_location_stock_location_id",
                table: "tbl_sale");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_stock_location_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "stock_location_id",
                table: "tbl_sale");

            migrationBuilder.AlterColumn<string>(
                name: "menu_name_kh",
                table: "tbl_menu",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");
        }
    }
}
