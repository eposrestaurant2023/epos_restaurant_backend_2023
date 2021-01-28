using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class removestation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_station_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_station");

            migrationBuilder.DropIndex(
                name: "IX_tbl_station_BusinessBranchModelid",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "BusinessBranchModelid",
                table: "tbl_station");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessBranchModelid",
                table: "tbl_station",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_BusinessBranchModelid",
                table: "tbl_station",
                column: "BusinessBranchModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_station_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_station",
                column: "BusinessBranchModelid",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
