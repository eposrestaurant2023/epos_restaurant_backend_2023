using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class drop_inventory_check_to_inv_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "inventory_check_id",
                table: "tbl_inventory_transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_inventory_check_id",
                table: "tbl_inventory_transaction",
                column: "inventory_check_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_inventory_check_inventory_check_id",
                table: "tbl_inventory_transaction",
                column: "inventory_check_id",
                principalTable: "tbl_inventory_check",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_inventory_check_inventory_check_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_transaction_inventory_check_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "inventory_check_id",
                table: "tbl_inventory_transaction");
        }
    }
}
