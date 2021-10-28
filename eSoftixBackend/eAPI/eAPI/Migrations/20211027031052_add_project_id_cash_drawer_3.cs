using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_project_id_cash_drawer_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cash_drawer_tbl_business_branch_project_business_branch_id",
                table: "tbl_cash_drawer");

            migrationBuilder.AlterColumn<Guid>(
                name: "project_business_branch_id",
                table: "tbl_cash_drawer",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_cash_drawer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cash_drawer_tbl_business_branch_project_business_branch_id",
                table: "tbl_cash_drawer",
                column: "project_business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cash_drawer_tbl_business_branch_project_business_branch_id",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_cash_drawer");

            migrationBuilder.AlterColumn<Guid>(
                name: "project_business_branch_id",
                table: "tbl_cash_drawer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cash_drawer_tbl_business_branch_project_business_branch_id",
                table: "tbl_cash_drawer",
                column: "project_business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
