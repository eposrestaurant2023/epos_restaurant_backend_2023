using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class salePayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "change_amount",
                table: "tbl_sale_payment",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "currency_name_en",
                table: "tbl_sale_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "currency_name_kh",
                table: "tbl_sale_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "payment_type_name_en",
                table: "tbl_sale_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "payment_type_name_kh",
                table: "tbl_sale_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "change_amount",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "currency_name_en",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "currency_name_kh",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "payment_type_name_en",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "payment_type_name_kh",
                table: "tbl_sale_payment");
        }
    }
}
