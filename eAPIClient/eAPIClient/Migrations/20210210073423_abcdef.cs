using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class abcdef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_customer_group",
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


            migrationBuilder.AddColumn<int>(
                name: "groupid",
                table: "tbl_customer_group",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group",
                column: "groupid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id",
                principalTable: "tbl_customer_group",
                principalColumn: "groupid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "groupid",
                table: "tbl_customer_group");

           

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

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id",
                principalTable: "tbl_customer_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
