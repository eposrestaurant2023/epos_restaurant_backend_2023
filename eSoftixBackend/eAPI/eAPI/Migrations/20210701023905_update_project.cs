using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "expired_date",
                table: "tbl_project",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_full_license",
                table: "tbl_project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "total_business_branches",
                table: "tbl_project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_outlets",
                table: "tbl_project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_stations",
                table: "tbl_project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_stock_location",
                table: "tbl_project",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expired_date",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "is_full_license",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "total_business_branches",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "total_outlets",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "total_stations",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "total_stock_location",
                table: "tbl_project");
        }
    }
}
