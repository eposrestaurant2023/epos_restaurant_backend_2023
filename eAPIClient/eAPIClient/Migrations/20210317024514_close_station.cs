using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class close_station : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "station_id",
                table: "tbl_working_day",
                newName: "opened_station_id");

            migrationBuilder.RenameColumn(
                name: "station_id",
                table: "tbl_cashier_shift",
                newName: "opened_station_id");

            migrationBuilder.AddColumn<Guid>(
                name: "closed_station_id",
                table: "tbl_working_day",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "closed_station_id",
                table: "tbl_cashier_shift",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "closed_station_id",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "closed_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.RenameColumn(
                name: "opened_station_id",
                table: "tbl_working_day",
                newName: "station_id");

            migrationBuilder.RenameColumn(
                name: "opened_station_id",
                table: "tbl_cashier_shift",
                newName: "station_id");
        }
    }
}
