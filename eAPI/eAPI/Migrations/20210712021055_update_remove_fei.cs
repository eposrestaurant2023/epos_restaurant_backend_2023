using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_remove_fei : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_tbl_stock_location_stock_location_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "stock_location_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "stock_locations",
                table: "tbl_product");

            migrationBuilder.AddColumn<Guid>(
                name: "station_id",
                table: "tbl_default_stock_location_product",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "station_id",
                table: "tbl_default_stock_location_product");

            migrationBuilder.AddColumn<Guid>(
                name: "stock_location_id",
                table: "tbl_sale_product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "stock_locations",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_stock_location_id",
                table: "tbl_sale_product",
                column: "stock_location_id");

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
