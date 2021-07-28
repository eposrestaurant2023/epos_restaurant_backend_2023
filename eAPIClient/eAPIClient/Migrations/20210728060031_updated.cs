using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "closed_station_name_en",
                table: "tbl_working_day",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "closed_station_name_kh",
                table: "tbl_working_day",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "opended_station_name_en",
                table: "tbl_working_day",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "opended_station_name_kh",
                table: "tbl_working_day",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "outlet_name_en",
                table: "tbl_working_day",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "outlet_name_kh",
                table: "tbl_working_day",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "closed_station_name_en",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "closed_station_name_kh",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "opened_station_name_en",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "opened_station_name_kh",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "outlet_name_en",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "outlet_name_kh",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "working_date",
                table: "tbl_cashier_shift",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "closed_station_name_en",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "closed_station_name_kh",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "opended_station_name_en",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "opended_station_name_kh",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "outlet_name_en",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "outlet_name_kh",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "closed_station_name_en",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "closed_station_name_kh",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "opened_station_name_en",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "opened_station_name_kh",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "outlet_name_en",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "outlet_name_kh",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "working_date",
                table: "tbl_cashier_shift");
        }
    }
}
