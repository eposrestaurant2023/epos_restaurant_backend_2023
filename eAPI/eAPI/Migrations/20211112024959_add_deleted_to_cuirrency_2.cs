using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_deleted_to_cuirrency_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_business_branch_currency",
                table: "tbl_business_branch_currency");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "tbl_business_branch_system_feature",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_currency_business_branch_id",
                table: "tbl_business_branch_currency",
                column: "business_branch_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_business_branch_currency_business_branch_id",
                table: "tbl_business_branch_currency");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tbl_business_branch_system_feature");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_business_branch_currency",
                table: "tbl_business_branch_currency",
                columns: new[] { "business_branch_id", "currency_id" });
        }
    }
}
