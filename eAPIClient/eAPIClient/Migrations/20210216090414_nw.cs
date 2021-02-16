using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class nw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_currency");

            migrationBuilder.AlterColumn<string>(
                name: "currency_name_kh",
                table: "tbl_currency",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "currency_name_en",
                table: "tbl_currency",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "currency_format",
                table: "tbl_currency",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "currency_name_kh",
                table: "tbl_currency",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "currency_name_en",
                table: "tbl_currency",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "currency_format",
                table: "tbl_currency",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_currency",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
