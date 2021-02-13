using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sjkdj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_CashierShiftShareModel_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropTable(
                name: "CashierShiftShareModel");

            migrationBuilder.AddColumn<decimal>(
                name: "close_amount",
                table: "tbl_cashier_shift",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "closed_by",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "closed_date",
                table: "tbl_cashier_shift",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_cashier_shift",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_cashier_shift",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_cashier_shift",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_cashier_shift",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "exchange_rate",
                table: "tbl_cashier_shift",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "is_closed",
                table: "tbl_cashier_shift",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tbl_cashier_shift",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "open_amount",
                table: "tbl_cashier_shift",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "shift_id",
                table: "tbl_cashier_shift",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_cashier_shift",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cashier_shift_shift_id",
                table: "tbl_cashier_shift",
                column: "shift_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_shift_shift_id",
                table: "tbl_cashier_shift",
                column: "shift_id",
                principalTable: "tbl_shift",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_shift_shift_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cashier_shift_shift_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "close_amount",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "closed_by",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "closed_date",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "exchange_rate",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "is_closed",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "open_amount",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "shift_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_cashier_shift");

            migrationBuilder.CreateTable(
                name: "CashierShiftShareModel",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    close_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    closed_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    closed_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    open_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    shift_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierShiftShareModel", x => x.id);
                    table.ForeignKey(
                        name: "FK_CashierShiftShareModel_tbl_shift_shift_id",
                        column: x => x.shift_id,
                        principalTable: "tbl_shift",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashierShiftShareModel_shift_id",
                table: "CashierShiftShareModel",
                column: "shift_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_CashierShiftShareModel_id",
                table: "tbl_cashier_shift",
                column: "id",
                principalTable: "CashierShiftShareModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
