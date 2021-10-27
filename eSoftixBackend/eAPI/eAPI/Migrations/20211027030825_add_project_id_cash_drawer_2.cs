using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_project_id_cash_drawer_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "project_business_branch_id",
                table: "tbl_cash_drawer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "project_id",
                table: "tbl_cash_drawer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cash_drawer_project_business_branch_id",
                table: "tbl_cash_drawer",
                column: "project_business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cash_drawer_project_id",
                table: "tbl_cash_drawer",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cash_drawer_tbl_business_branch_project_business_branch_id",
                table: "tbl_cash_drawer",
                column: "project_business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cash_drawer_tbl_project_project_id",
                table: "tbl_cash_drawer",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cash_drawer_tbl_business_branch_project_business_branch_id",
                table: "tbl_cash_drawer");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cash_drawer_tbl_project_project_id",
                table: "tbl_cash_drawer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cash_drawer_project_business_branch_id",
                table: "tbl_cash_drawer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cash_drawer_project_id",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "project_business_branch_id",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "tbl_cash_drawer");
        }
    }
}
