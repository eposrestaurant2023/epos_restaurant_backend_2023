using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class remove_total_balance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payment_amount",
                table: "tbl_coupon_voucher_transaction");

            migrationBuilder.DropColumn(
                name: "total_balance",
                table: "tbl_coupon_voucher_transaction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "payment_amount",
                table: "tbl_coupon_voucher_transaction",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_balance",
                table: "tbl_coupon_voucher_transaction",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
