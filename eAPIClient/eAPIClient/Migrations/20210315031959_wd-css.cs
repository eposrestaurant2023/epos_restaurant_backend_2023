using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class wdcss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cashier_shift_number",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "working_day_number",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cashier_shift_number",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "working_day_number",
                table: "tbl_sale");
        }
    }
}
