using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_inventory_checkj_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "inventory_check_date",
                table: "tbl_inventory_check_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "inventory_check_date",
                table: "tbl_inventory_check_product");
        }
    }
}
