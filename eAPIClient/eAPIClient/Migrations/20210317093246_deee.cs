using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class deee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ip_address_port",
                table: "tbl_product_printer",
                newName: "ip_address");

            migrationBuilder.AddColumn<string>(
                name: "discount_code",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "discount_note",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "total_credit",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

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
                name: "discount_code",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "discount_note",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "total_credit",
                table: "tbl_sale");

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
