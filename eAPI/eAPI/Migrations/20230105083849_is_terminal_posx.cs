using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class is_terminal_posx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_termial_pos",
                table: "tbl_station",
                newName: "is_terminal_pos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_terminal_pos",
                table: "tbl_station",
                newName: "is_termial_pos");
        }
    }
}
