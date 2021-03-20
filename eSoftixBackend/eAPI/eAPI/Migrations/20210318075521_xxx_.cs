using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class xxx_ : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
