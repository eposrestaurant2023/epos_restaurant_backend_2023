using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class salePaymentx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sale_payment_exchange_rates",
                table: "tbl_sale_payment");

            migrationBuilder.AddColumn<int>(
                name: "currency_id",
                table: "tbl_sale_payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "currency_exchange_rate_data",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency_id",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "currency_exchange_rate_data",
                table: "tbl_sale");

            migrationBuilder.AddColumn<string>(
                name: "sale_payment_exchange_rates",
                table: "tbl_sale_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }
    }
}
