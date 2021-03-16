using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_tbl_tation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_station_tbl_outlet_outlet_id",
                table: "tbl_station");

            migrationBuilder.DropTable(
                name: "tbl_outlet_station");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_station",
                table: "tbl_station");

            migrationBuilder.RenameTable(
                name: "tbl_station",
                newName: "StationModel");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_station_outlet_id",
                table: "StationModel",
                newName: "IX_StationModel_outlet_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StationModel",
                table: "StationModel",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_StationModel_tbl_outlet_outlet_id",
                table: "StationModel",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
