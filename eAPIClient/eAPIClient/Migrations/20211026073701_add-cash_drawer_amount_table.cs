using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class addcash_drawer_amount_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_cash_drawer_amount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    working_day_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    last_modified_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cashier_shift_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cash_drawer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    currency_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    format = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    exchange_rate = table.Column<double>(type: "float", nullable: false),
                    transaction_type_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    base_currency_amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    base_currency_format = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    multiplier_value = table.Column<int>(type: "int", nullable: false),
                    cash_deposit_to = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cash_drawer_amount", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_cash_drawer_amount_tbl_working_day_working_day_id",
                        column: x => x.working_day_id,
                        principalTable: "tbl_working_day",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cash_drawer_amount_working_day_id",
                table: "tbl_cash_drawer_amount",
                column: "working_day_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_cash_drawer_amount");
        }
    }
}
