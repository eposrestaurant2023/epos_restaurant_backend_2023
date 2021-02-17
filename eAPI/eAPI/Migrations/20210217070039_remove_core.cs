using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class remove_core : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_unit_category");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_unit_category");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_unit_category");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_unit_category");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_unit_category");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_unit_category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_unit_category",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_unit_category",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_unit_category",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_unit_category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tbl_unit_category",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_unit_category",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
