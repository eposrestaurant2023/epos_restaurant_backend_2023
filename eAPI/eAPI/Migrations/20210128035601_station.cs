using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class station : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_station_tbl_business_branch_business_branch_id",
                table: "tbl_station");

            migrationBuilder.DropIndex(
                name: "IX_tbl_station_business_branch_id",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_station");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_station",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_business_branch_id",
                table: "tbl_station",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_station_tbl_business_branch_business_branch_id",
                table: "tbl_station",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
