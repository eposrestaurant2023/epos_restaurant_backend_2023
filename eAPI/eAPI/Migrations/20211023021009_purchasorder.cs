using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class purchasorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency_format",
                table: "tbl_purchase_order_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "currency_id",
                table: "tbl_purchase_order_payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "currency_name_en",
                table: "tbl_purchase_order_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "currency_name_kh",
                table: "tbl_purchase_order_payment",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<double>(
                name: "exchange_rate",
                table: "tbl_purchase_order_payment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency_format",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "currency_id",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "currency_name_en",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "currency_name_kh",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "exchange_rate",
                table: "tbl_purchase_order_payment");
        }
    }
}
