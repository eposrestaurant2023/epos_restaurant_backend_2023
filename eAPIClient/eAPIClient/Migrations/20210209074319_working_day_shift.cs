using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class working_day_shift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_product_portion",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product_portion",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_modifier",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.CreateTable(
                name: "tbl_working_day",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    working_date = table.Column<DateTime>(type: "date", nullable: false),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    closed_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    closed_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_working_day", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_cashier_shift",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    working_day_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    closed_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    closed_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    open_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    close_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cashier_shift", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_cashier_shift_tbl_working_day_working_day_id",
                        column: x => x.working_day_id,
                        principalTable: "tbl_working_day",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cashier_shift_working_day_id",
                table: "tbl_cashier_shift",
                column: "working_day_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_cashier_shift");

            migrationBuilder.DropTable(
                name: "tbl_working_day");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_product_portion",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cost",
                table: "tbl_product_portion",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "tbl_product_modifier",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");
        }
    }
}
