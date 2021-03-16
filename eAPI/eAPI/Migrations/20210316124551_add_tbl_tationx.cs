using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_tbl_tationx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "tbl_station",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_already_config = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
                    station_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    station_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_station", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_station_tbl_outlet_outlet_id",
                        column: x => x.outlet_id,
                        principalTable: "tbl_outlet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_outlet_id",
                table: "tbl_station",
                column: "outlet_id");
        }
    }
}
