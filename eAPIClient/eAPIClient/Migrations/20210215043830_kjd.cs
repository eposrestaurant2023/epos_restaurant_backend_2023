using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class kjd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_customer");

            migrationBuilder.AddColumn<Guid>(
                name: "business_id",
                table: "tbl_customer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "business_id",
                table: "tbl_customer");

            migrationBuilder.AddColumn<int>(
                name: "business_branch_id",
                table: "tbl_customer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
