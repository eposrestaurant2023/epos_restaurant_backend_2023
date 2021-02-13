using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_stock_transfer_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_tbl_business_branch_form_business_branch_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_tbl_stock_location_form_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_form_business_branch_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_form_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "form_business_branch_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "form_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_from_business_branch_id",
                table: "tbl_stock_transfer",
                column: "from_business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_from_stock_location_id",
                table: "tbl_stock_transfer",
                column: "from_stock_location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_tbl_business_branch_from_business_branch_id",
                table: "tbl_stock_transfer",
                column: "from_business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_tbl_stock_location_from_stock_location_id",
                table: "tbl_stock_transfer",
                column: "from_stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_tbl_business_branch_from_business_branch_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_tbl_stock_location_from_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_from_business_branch_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_from_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.AddColumn<Guid>(
                name: "form_business_branch_id",
                table: "tbl_stock_transfer",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "form_stock_location_id",
                table: "tbl_stock_transfer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_form_business_branch_id",
                table: "tbl_stock_transfer",
                column: "form_business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_form_stock_location_id",
                table: "tbl_stock_transfer",
                column: "form_stock_location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_tbl_business_branch_form_business_branch_id",
                table: "tbl_stock_transfer",
                column: "form_business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_tbl_stock_location_form_stock_location_id",
                table: "tbl_stock_transfer",
                column: "form_stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
