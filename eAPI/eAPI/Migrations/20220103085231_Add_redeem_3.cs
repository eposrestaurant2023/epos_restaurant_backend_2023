using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class Add_redeem_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "park_sale_product_id",
                table: "tbl_sale_product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "redeem_sale_product_id",
                table: "tbl_sale_product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "park_sale_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "redeem_park_sale_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "park_sale_product_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "redeem_sale_product_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "park_sale_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "redeem_park_sale_id",
                table: "tbl_sale");
        }
    }
}
