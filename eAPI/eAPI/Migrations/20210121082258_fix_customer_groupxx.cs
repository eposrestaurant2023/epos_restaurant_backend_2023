using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_customer_groupxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "tbl_customer_group",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_customer_group",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_customer_group",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_customer_group",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_customer_group",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tbl_customer_group",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_customer_group",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_customer_group");
        }
    }
}
