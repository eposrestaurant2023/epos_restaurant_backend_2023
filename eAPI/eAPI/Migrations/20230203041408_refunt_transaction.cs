using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class refunt_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_refund_transaction",
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
                    coupon_transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    working_day_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cash_drawer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cashier_shift_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_refund_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_refund_transaction_tbl_coupon_voucher_transaction_coupon_transaction_id",
                        column: x => x.coupon_transaction_id,
                        principalTable: "tbl_coupon_voucher_transaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_refund_transaction_coupon_transaction_id",
                table: "tbl_refund_transaction",
                column: "coupon_transaction_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_refund_transaction");
        }
    }
}
