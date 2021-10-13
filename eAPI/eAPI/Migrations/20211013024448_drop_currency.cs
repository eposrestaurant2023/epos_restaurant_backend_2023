using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class drop_currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "default_change_exchange_rate",
                table: "tbl_currency");

            migrationBuilder.DropColumn(
                name: "default_exchange_rate",
                table: "tbl_currency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "default_change_exchange_rate",
                table: "tbl_currency",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "default_exchange_rate",
                table: "tbl_currency",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
