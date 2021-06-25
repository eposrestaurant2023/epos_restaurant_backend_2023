using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_db_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "current_balance",
                table: "tbl_customer",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "expired_date",
                table: "tbl_customer",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "pending_project",
                table: "tbl_customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_project",
                table: "tbl_customer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_balance",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "expired_date",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "pending_project",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "total_project",
                table: "tbl_customer");
        }
    }
}
