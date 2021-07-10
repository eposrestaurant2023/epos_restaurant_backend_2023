using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class payment_exchange_rate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "exchange_rate",
                table: "tbl_sale_payment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "sale_payment_exchange_rates",
                table: "tbl_sale_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exchange_rate",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "sale_payment_exchange_rates",
                table: "tbl_sale_payment");
        }
    }
}
