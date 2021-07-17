using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class expirddate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_tbl_project_project_id",
                table: "tbl_business_branch");

            migrationBuilder.DropIndex(
                name: "IX_tbl_business_branch_project_id",
                table: "tbl_business_branch");

            migrationBuilder.AddColumn<Guid>(
                name: "Projectid",
                table: "tbl_business_branch",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expired_date",
                table: "tbl_business_branch",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_Projectid",
                table: "tbl_business_branch",
                column: "Projectid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_tbl_project_Projectid",
                table: "tbl_business_branch",
                column: "Projectid",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_tbl_project_Projectid",
                table: "tbl_business_branch");

            migrationBuilder.DropIndex(
                name: "IX_tbl_business_branch_Projectid",
                table: "tbl_business_branch");

            migrationBuilder.DropColumn(
                name: "Projectid",
                table: "tbl_business_branch");

            migrationBuilder.DropColumn(
                name: "expired_date",
                table: "tbl_business_branch");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_project_id",
                table: "tbl_business_branch",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_tbl_project_project_id",
                table: "tbl_business_branch",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
