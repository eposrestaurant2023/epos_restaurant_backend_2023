using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_stock_location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_location_tbl_outlet_outlet_id",
                table: "tbl_stock_location");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_location_outlet_id",
                table: "tbl_stock_location");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_stock_location");

            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_stock_location",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_business_branch_id",
                table: "tbl_stock_location",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_location_tbl_business_branch_business_branch_id",
                table: "tbl_stock_location",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_location_tbl_business_branch_business_branch_id",
                table: "tbl_stock_location");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_location_business_branch_id",
                table: "tbl_stock_location");

            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_stock_location");

            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_stock_location",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_outlet_id",
                table: "tbl_stock_location",
                column: "outlet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_location_tbl_outlet_outlet_id",
                table: "tbl_stock_location",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
