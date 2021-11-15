using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class motifynotetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category_note_id",
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
                name: "is_deleted",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "note_id",
                table: "tbl_note");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_note");

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "tbl_note",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "tbl_note");

            migrationBuilder.AddColumn<int>(
                name: "category_note_id",
                table: "tbl_note",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tbl_note",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<int>(
                name: "note_id",
                table: "tbl_note",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_note",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
