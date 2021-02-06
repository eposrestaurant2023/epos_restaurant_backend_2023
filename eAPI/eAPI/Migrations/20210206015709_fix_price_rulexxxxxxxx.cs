using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_price_rulexxxxxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "tbl_business_branch_product_price",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_business_branch_product_price",
                table: "tbl_business_branch_product_price",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_business_branch_product_price",
                table: "tbl_business_branch_product_price");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tbl_business_branch_product_price");
        }
    }
}
