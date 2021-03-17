using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_tbl_tationxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_station",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
