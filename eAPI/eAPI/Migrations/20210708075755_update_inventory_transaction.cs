using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_inventory_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "portion_id",
                table: "tbl_inventory_transaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "sale_product_id",
                table: "tbl_inventory_transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "sale_product_modifier_id",
                table: "tbl_inventory_transaction",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "portion_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "sale_product_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "sale_product_modifier_id",
                table: "tbl_inventory_transaction");
        }
    }
}
