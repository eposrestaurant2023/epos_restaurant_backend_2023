using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_require_check_in_check_out : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "check_in_by",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "check_in_date",
                table: "tbl_sale",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "check_out_by",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "check_out_date",
                table: "tbl_sale",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "check_in_by",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "check_in_date",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "check_out_by",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "check_out_date",
                table: "tbl_sale");
        }
    }
}
