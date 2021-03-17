using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class printersd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ip_address_port",
                table: "tbl_product_printer",
                newName: "ip_address");

            migrationBuilder.AddColumn<int>(
                name: "port",
                table: "tbl_product_printer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "port",
                table: "tbl_product_printer");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "tbl_product_printer",
                newName: "ip_address_port");
        }
    }
}
