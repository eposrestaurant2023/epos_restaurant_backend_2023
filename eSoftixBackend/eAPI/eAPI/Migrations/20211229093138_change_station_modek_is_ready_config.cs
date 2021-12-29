using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_station_modek_is_ready_config : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_ready_config",
                table: "tbl_station",
                newName: "is_already_config");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_already_config",
                table: "tbl_station",
                newName: "is_ready_config");
        }
    }
}
