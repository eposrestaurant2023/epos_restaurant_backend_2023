using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_payment_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "payment_typeid",
                table: "tbl_prefix_price",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CurrencyShareModel",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    currency_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_format = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    change_exchange_rate = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyShareModel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    currency_id = table.Column<int>(type: "int", nullable: true),
                    payment_type_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    is_build_in = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    is_credit = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.id);
                    table.ForeignKey(
                        name: "FK_PaymentTypes_CurrencyShareModel_currency_id",
                        column: x => x.currency_id,
                        principalTable: "CurrencyShareModel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_prefix_price_payment_typeid",
                table: "tbl_prefix_price",
                column: "payment_typeid");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_currency_id",
                table: "PaymentTypes",
                column: "currency_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_prefix_price_PaymentTypes_payment_typeid",
                table: "tbl_prefix_price",
                column: "payment_typeid",
                principalTable: "PaymentTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_prefix_price_PaymentTypes_payment_typeid",
                table: "tbl_prefix_price");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "CurrencyShareModel");

            migrationBuilder.DropIndex(
                name: "IX_tbl_prefix_price_payment_typeid",
                table: "tbl_prefix_price");

            migrationBuilder.DropColumn(
                name: "payment_typeid",
                table: "tbl_prefix_price");
        }
    }
}
