using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_config_dataxxdx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_currency_tbl_business_branch_business_branch_id",
                table: "tbl_currency");

            migrationBuilder.DropIndex(
                name: "IX_tbl_currency_business_branch_id",
                table: "tbl_currency");

            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_currency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_currency",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_currency_business_branch_id",
                table: "tbl_currency",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_currency_tbl_business_branch_business_branch_id",
                table: "tbl_currency",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
