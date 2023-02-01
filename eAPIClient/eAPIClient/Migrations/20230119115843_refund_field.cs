using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class refund_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "refund_cash_drawer_id",
                table: "tbl_coupon_voucher_transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "refund_cashier_shift_id",
                table: "tbl_coupon_voucher_transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "refund_working_day_id",
                table: "tbl_coupon_voucher_transaction",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refund_cash_drawer_id",
                table: "tbl_coupon_voucher_transaction");

            migrationBuilder.DropColumn(
                name: "refund_cashier_shift_id",
                table: "tbl_coupon_voucher_transaction");

            migrationBuilder.DropColumn(
                name: "refund_working_day_id",
                table: "tbl_coupon_voucher_transaction");
        }
    }
}
