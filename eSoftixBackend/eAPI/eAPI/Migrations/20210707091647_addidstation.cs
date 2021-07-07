using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class addidstation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "extend_date",
                table: "tbl_extend_liscense_history",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "station_id",
                table: "tbl_extend_liscense_history",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_extend_liscense_history_station_id",
                table: "tbl_extend_liscense_history",
                column: "station_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_extend_liscense_history_tbl_station_station_id",
                table: "tbl_extend_liscense_history",
                column: "station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_extend_liscense_history_tbl_station_station_id",
                table: "tbl_extend_liscense_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_extend_liscense_history_station_id",
                table: "tbl_extend_liscense_history");

            migrationBuilder.DropColumn(
                name: "station_id",
                table: "tbl_extend_liscense_history");

            migrationBuilder.AlterColumn<DateTime>(
                name: "extend_date",
                table: "tbl_extend_liscense_history",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
