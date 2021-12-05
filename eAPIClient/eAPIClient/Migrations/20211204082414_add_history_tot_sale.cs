using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_history_tot_sale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "saleid",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_saleid",
                table: "tbl_history",
                column: "saleid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_sale_saleid",
                table: "tbl_history",
                column: "saleid",
                principalTable: "tbl_sale",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_sale_saleid",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_saleid",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "saleid",
                table: "tbl_history");
        }
    }
}
