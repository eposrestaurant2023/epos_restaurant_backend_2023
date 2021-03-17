using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class printers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ip_address_port",
                table: "tbl_printer",
                newName: "ip_address");

            migrationBuilder.AlterColumn<int>(
                name: "port",
                table: "tbl_printer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_predefine_discount_code");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "tbl_printer",
                newName: "ip_address_port");

            migrationBuilder.AlterColumn<string>(
                name: "port",
                table: "tbl_printer",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
