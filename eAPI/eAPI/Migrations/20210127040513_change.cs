using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_outlet_station_tbl_outlet_outlet_id",
                table: "tbl_outlet_station");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_outlet_station_tbl_station_station_id",
                table: "tbl_outlet_station");

            migrationBuilder.DropIndex(
                name: "IX_tbl_outlet_station_outlet_id",
                table: "tbl_outlet_station");

            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_station",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_outlet_id",
                table: "tbl_station",
                column: "outlet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_station_tbl_outlet_outlet_id",
                table: "tbl_station",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_station_tbl_outlet_outlet_id",
                table: "tbl_station");

            migrationBuilder.DropIndex(
                name: "IX_tbl_station_outlet_id",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_station");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_outlet_station_outlet_id",
                table: "tbl_outlet_station",
                column: "outlet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_outlet_station_tbl_outlet_outlet_id",
                table: "tbl_outlet_station",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_outlet_station_tbl_station_station_id",
                table: "tbl_outlet_station",
                column: "station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
