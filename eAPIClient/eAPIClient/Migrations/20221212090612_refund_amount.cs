using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class refund_amount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "refund_amount",
                table: "tbl_coupon_voucher_transaction",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_refund_amount",
                table: "tbl_coupon_voucher",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refund_amount",
                table: "tbl_coupon_voucher_transaction");

            migrationBuilder.DropColumn(
                name: "total_refund_amount",
                table: "tbl_coupon_voucher");
        }
    }
}
