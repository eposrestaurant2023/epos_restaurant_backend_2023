using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_currency_add_tbl_bbc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "change_exchange_rate",
                table: "tbl_currency");

            migrationBuilder.DropColumn(
                name: "exchange_rate",
                table: "tbl_currency");

            migrationBuilder.AddColumn<bool>(
                name: "is_base_exchange_currency",
                table: "tbl_currency",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "tbl_business_branch_currency",
                columns: table => new
                {
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    exchange_rate_input = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    change_exchange_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    change_exchange_rate_input = table.Column<decimal>(type: "decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_business_branch_currency", x => new { x.business_branch_id, x.currency_id });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_business_branch_currency");

            migrationBuilder.DropColumn(
                name: "is_base_exchange_currency",
                table: "tbl_currency");

            migrationBuilder.AddColumn<decimal>(
                name: "change_exchange_rate",
                table: "tbl_currency",
                type: "decimal(19,10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "exchange_rate",
                table: "tbl_currency",
                type: "decimal(19,10)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
