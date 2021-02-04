using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_sale_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_business_branch_id",
                table: "tbl_sale",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_business_branch_business_branch_id",
                table: "tbl_sale",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_business_branch_business_branch_id",
                table: "tbl_sale");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_business_branch_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_sale");

            migrationBuilder.AddColumn<int>(
                name: "stock_location_id",
                table: "tbl_sale",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
