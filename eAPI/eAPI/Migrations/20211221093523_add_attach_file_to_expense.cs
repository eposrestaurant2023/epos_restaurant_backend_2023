using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_attach_file_to_expense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<Guid>(
                name: "expense_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_expense_id",
                table: "tbl_attach_files",
                column: "expense_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_expense_expense_id",
                table: "tbl_attach_files",
                column: "expense_id",
                principalTable: "tbl_expense",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_expense_expense_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_expense_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "is_order_station",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "revenue_group_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "expense_id",
                table: "tbl_attach_files");
        }
    }
}
