using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class remove_field_balanc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "balance",
                table: "tbl_coupon_voucher_transaction");

            migrationBuilder.DropColumn(
                name: "base_balance",
                table: "tbl_coupon_voucher_transaction");

            migrationBuilder.AddColumn<bool>(
                name: "is_used",
                table: "tbl_coupon_voucher_transaction",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_used",
                table: "tbl_coupon_voucher_transaction");

            migrationBuilder.AddColumn<decimal>(
                name: "balance",
                table: "tbl_coupon_voucher_transaction",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "base_balance",
                table: "tbl_coupon_voucher_transaction",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
