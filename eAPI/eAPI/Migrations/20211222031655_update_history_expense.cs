using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_history_expense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
