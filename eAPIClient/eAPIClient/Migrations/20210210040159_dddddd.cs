using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class dddddd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "is_build_in",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_payment_type");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "tbl_user");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_station",
                newName: "tableid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_payment_type",
                newName: "tableid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "OutletModel",
                newName: "tableid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_user",
                newName: "tableid");

            migrationBuilder.AlterColumn<string>(
                name: "payment_type_name_en",
                table: "tbl_payment_type",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "tbl_user",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_user",
                table: "tbl_user",
                column: "tableid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_user",
                table: "tbl_user");

            migrationBuilder.RenameTable(
                name: "tbl_user",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "tableid",
                table: "tbl_station",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "tableid",
                table: "tbl_payment_type",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "tableid",
                table: "OutletModel",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "tableid",
                table: "Users",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_station",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_station",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_station",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_station",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tbl_station",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_station",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "payment_type_name_en",
                table: "tbl_payment_type",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_payment_type",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_payment_type",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_payment_type",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_payment_type",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_build_in",
                table: "tbl_payment_type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tbl_payment_type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_payment_type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");
        }
    }
}
