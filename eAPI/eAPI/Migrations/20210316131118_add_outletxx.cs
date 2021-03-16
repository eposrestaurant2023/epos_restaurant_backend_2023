using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_outletxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_outlet_station");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_sale");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_sale",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbl_outlet_station",
                columns: table => new
                {
                    station_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    outlet_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_outlet_station", x => new { x.station_id, x.outlet_id });
                });
        }
    }
}
