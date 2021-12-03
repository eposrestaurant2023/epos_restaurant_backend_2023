using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class attechfileinventorycheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "inventory_check_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_inventory_check_id",
                table: "tbl_attach_files",
                column: "inventory_check_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_inventory_check_inventory_check_id",
                table: "tbl_attach_files",
                column: "inventory_check_id",
                principalTable: "tbl_inventory_check",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_inventory_check_inventory_check_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_inventory_check_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "inventory_check_id",
                table: "tbl_attach_files");
        }
    }
}
