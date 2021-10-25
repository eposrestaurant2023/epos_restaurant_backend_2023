using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_cash_drawer_amount4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "base_currency_format",
                table: "tbl_cash_drawer_amount",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "base_currency_format",
                table: "tbl_cash_drawer_amount",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");
        }
    }
}
