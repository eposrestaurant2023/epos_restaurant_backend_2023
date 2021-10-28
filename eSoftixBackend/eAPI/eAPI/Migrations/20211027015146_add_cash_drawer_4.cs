using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_cash_drawer_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_cash_drawer");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_cash_drawer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_cash_drawer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_cash_drawer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_cash_drawer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_cash_drawer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tbl_cash_drawer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_cash_drawer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_cash_drawer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_cash_drawer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
