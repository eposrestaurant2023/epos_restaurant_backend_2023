using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class xxd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "exchange_rate_input",
                table: "tbl_business_branch_currency",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<double>(
                name: "exchange_rate",
                table: "tbl_business_branch_currency",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<double>(
                name: "change_exchange_rate_input",
                table: "tbl_business_branch_currency",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

            migrationBuilder.AlterColumn<double>(
                name: "change_exchange_rate",
                table: "tbl_business_branch_currency",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "exchange_rate_input",
                table: "tbl_business_branch_currency",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "exchange_rate",
                table: "tbl_business_branch_currency",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "change_exchange_rate_input",
                table: "tbl_business_branch_currency",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "change_exchange_rate",
                table: "tbl_business_branch_currency",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
