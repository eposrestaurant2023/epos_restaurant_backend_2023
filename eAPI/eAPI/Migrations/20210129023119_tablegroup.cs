using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class tablegroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_table_group_tbl_station_station_id",
                table: "tbl_table_group");

            migrationBuilder.RenameColumn(
                name: "station_id",
                table: "tbl_table_group",
                newName: "outlet_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_table_group_station_id",
                table: "tbl_table_group",
                newName: "IX_tbl_table_group_outlet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_table_group_tbl_outlet_outlet_id",
                table: "tbl_table_group",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_table_group_tbl_outlet_outlet_id",
                table: "tbl_table_group");

            migrationBuilder.RenameColumn(
                name: "outlet_id",
                table: "tbl_table_group",
                newName: "station_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_table_group_outlet_id",
                table: "tbl_table_group",
                newName: "IX_tbl_table_group_station_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_table_group_tbl_station_station_id",
                table: "tbl_table_group",
                column: "station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
