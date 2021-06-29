using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_relative_tbl_cashier_shift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "open_station_id",
                table: "tbl_cashier_shift",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cashier_shift_closed_station_id",
                table: "tbl_cashier_shift",
                column: "closed_station_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cashier_shift_open_station_id",
                table: "tbl_cashier_shift",
                column: "open_station_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cashier_shift_outlet_id",
                table: "tbl_cashier_shift",
                column: "outlet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_outlet_outlet_id",
                table: "tbl_cashier_shift",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_station_closed_station_id",
                table: "tbl_cashier_shift",
                column: "closed_station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_station_open_station_id",
                table: "tbl_cashier_shift",
                column: "open_station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_outlet_outlet_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_station_closed_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_station_open_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cashier_shift_closed_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cashier_shift_open_station_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cashier_shift_outlet_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "open_station_id",
                table: "tbl_cashier_shift");
        }
    }
}
