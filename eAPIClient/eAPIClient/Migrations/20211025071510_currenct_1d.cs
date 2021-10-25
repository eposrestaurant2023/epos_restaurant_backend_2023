using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class currenct_1d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "last_modified_by",
                table: "tbl_sale",
                newName: "deleted_note");

            migrationBuilder.AddColumn<Guid>(
                name: "cash_drawer_id",
                table: "tbl_working_day",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "currency_format",
                table: "tbl_sale_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<bool>(
                name: "is_foc",
                table: "tbl_sale",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cash_drawer_id",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "currency_format",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "is_foc",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "deleted_note",
                table: "tbl_sale",
                newName: "last_modified_by");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_sale",
                type: "datetime2",
                nullable: true);
        }
    }
}
