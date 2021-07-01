using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_business_branchj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessBranchModelid",
                table: "tbl_system_feature",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_system_feature_BusinessBranchModelid",
                table: "tbl_system_feature",
                column: "BusinessBranchModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_system_feature_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_system_feature",
                column: "BusinessBranchModelid",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_system_feature_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_system_feature");

            migrationBuilder.DropIndex(
                name: "IX_tbl_system_feature_BusinessBranchModelid",
                table: "tbl_system_feature");

            migrationBuilder.DropColumn(
                name: "BusinessBranchModelid",
                table: "tbl_system_feature");
        }
    }
}
