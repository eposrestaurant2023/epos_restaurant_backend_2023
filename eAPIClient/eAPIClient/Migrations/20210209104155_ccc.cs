using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class ccc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_station",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
                    station_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    station_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    is_already_config = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_station", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_station_OutletModel_outlet_id",
                        column: x => x.outlet_id,
                        principalTable: "OutletModel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_outlet_id",
                table: "tbl_station",
                column: "outlet_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_station");
        }
    }
}
