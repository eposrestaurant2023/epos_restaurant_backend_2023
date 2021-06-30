using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_relative_tbl_working_day : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_working_day_closed_station_id",
                table: "tbl_working_day",
                column: "closed_station_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_working_day_opened_station_id",
                table: "tbl_working_day",
                column: "opened_station_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_working_day_outlet_id",
                table: "tbl_working_day",
                column: "outlet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_working_day_tbl_outlet_outlet_id",
                table: "tbl_working_day",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_working_day_tbl_station_closed_station_id",
                table: "tbl_working_day",
                column: "closed_station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_working_day_tbl_station_opened_station_id",
                table: "tbl_working_day",
                column: "opened_station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_working_day_tbl_outlet_outlet_id",
                table: "tbl_working_day");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_working_day_tbl_station_closed_station_id",
                table: "tbl_working_day");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_working_day_tbl_station_opened_station_id",
                table: "tbl_working_day");

            migrationBuilder.DropIndex(
                name: "IX_tbl_working_day_closed_station_id",
                table: "tbl_working_day");

            migrationBuilder.DropIndex(
                name: "IX_tbl_working_day_opened_station_id",
                table: "tbl_working_day");

            migrationBuilder.DropIndex(
                name: "IX_tbl_working_day_outlet_id",
                table: "tbl_working_day");
        }
    }
}
