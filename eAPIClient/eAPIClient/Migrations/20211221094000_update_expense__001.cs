using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class update_expense__001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "expense_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_expense_id",
                table: "tbl_history",
                column: "expense_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_expense_expense_id",
                table: "tbl_history",
                column: "expense_id",
                principalTable: "tbl_expense",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_expense_expense_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_expense_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "expense_id",
                table: "tbl_history");
        }
    }
}
