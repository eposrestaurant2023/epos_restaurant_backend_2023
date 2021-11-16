using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_notex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_note",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_note");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "tbl_note",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_note",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_note",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_note",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_note",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_note",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_note",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_note",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_note",
                table: "tbl_note",
                column: "id");
        }
    }
}
