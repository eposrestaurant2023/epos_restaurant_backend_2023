using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_parent_child_modi_group_item_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_modifier_group_item",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_modifier_group_item",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_modifier_group_item",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_modifier_group_item",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_modifier_group_item",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_modifier_group_item");
        }
    }
}
