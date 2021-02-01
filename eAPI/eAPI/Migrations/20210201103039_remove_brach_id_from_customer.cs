using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class remove_brach_id_from_customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_customer_tbl_business_branch_business_branch_id",
                table: "tbl_customer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_customer_business_branch_id",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_customer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_customer_business_branch_id",
                table: "tbl_customer",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_customer_tbl_business_branch_business_branch_id",
                table: "tbl_customer",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
