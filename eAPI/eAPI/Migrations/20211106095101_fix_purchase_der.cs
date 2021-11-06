using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_purchase_der : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "change_exchange_rate",
                table: "tbl_purchase_order_payment");

            migrationBuilder.DropColumn(
                name: "is_create_payment_in_puchase_order",
                table: "tbl_purchase_order_payment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "change_exchange_rate",
                table: "tbl_purchase_order_payment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "is_create_payment_in_puchase_order",
                table: "tbl_purchase_order_payment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
