using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_price_rulexx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "port",
                table: "tbl_printer");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "tbl_printer",
                newName: "ip_address_port");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ip_address_port",
                table: "tbl_printer",
                newName: "ip_address");

            migrationBuilder.AddColumn<int>(
                name: "port",
                table: "tbl_printer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
