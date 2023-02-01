using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class cpv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "order_station_allow_payment",
                table: "tbl_station",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "coupon_number",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "coupon_voucher_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "coupon_voucher_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "temp_coupon_voucher_amount",
                table: "tbl_sale",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "tbl_coupon_voucher",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    last_modified_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    registered_business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    registered_business_branch_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    registered_business_branch_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    coupon_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    registered_date = table.Column<DateTime>(type: "date", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "date", nullable: false),
                    total_balance = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    total_refund_amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    is_synced = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_coupon_voucher", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_coupon_voucher_transaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    last_modified_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    coupon_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    coupon_voucher_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_branch_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    business_branch_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    outlet_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    outlet_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    station_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    station_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    station_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    cash_drawer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cash_drawer_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    working_day_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    working_day_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    working_date = table.Column<DateTime>(type: "date", nullable: false),
                    cashier_shift_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cashier_shift_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    shift_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    top_up_amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    base_top_up_amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    refund_amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    current_balance = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    base_current_balance = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    currency_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    currency_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    exchange_rate = table.Column<double>(type: "float", nullable: false),
                    change_exchange_rate = table.Column<double>(type: "float", nullable: false),
                    symbol = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_prefix_symbol = table.Column<bool>(type: "bit", nullable: false),
                    payment_type_group = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    payment_type_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_synced = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_coupon_voucher_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_coupon_voucher_transaction_tbl_coupon_voucher_coupon_voucher_id",
                        column: x => x.coupon_voucher_id,
                        principalTable: "tbl_coupon_voucher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_coupon_voucher_transaction_coupon_voucher_id",
                table: "tbl_coupon_voucher_transaction",
                column: "coupon_voucher_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_coupon_voucher_transaction");

            migrationBuilder.DropTable(
                name: "tbl_coupon_voucher");

            migrationBuilder.DropColumn(
                name: "order_station_allow_payment",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "coupon_number",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "coupon_voucher_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "coupon_voucher_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "temp_coupon_voucher_amount",
                table: "tbl_sale");
        }
    }
}
