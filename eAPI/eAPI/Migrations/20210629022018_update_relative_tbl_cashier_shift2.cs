using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_relative_tbl_cashier_shift2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_station_open_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cashier_shift_open_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "open_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cashier_shift_opened_station_id",
                table: "tbl_cashier_shift",
                column: "opened_station_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_station_opened_station_id",
                table: "tbl_cashier_shift",
                column: "opened_station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_station_opened_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cashier_shift_opened_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.AddColumn<Guid>(
                name: "open_station_id",
                table: "tbl_cashier_shift",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cashier_shift_open_station_id",
                table: "tbl_cashier_shift",
                column: "open_station_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_station_open_station_id",
                table: "tbl_cashier_shift",
                column: "open_station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
