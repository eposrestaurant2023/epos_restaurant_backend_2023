using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class bew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "modifier_id",
                table: "tbl_sale_product_modifier",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "modifier_id",
                table: "tbl_sale_product_modifier");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "tbl_product_modifier");
        }
    }
}
