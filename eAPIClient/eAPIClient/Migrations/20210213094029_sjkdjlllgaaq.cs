using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sjkdjlllgaaq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "close_note",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "open_note",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "close_note",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "open_note",
                table: "tbl_cashier_shift");
        }
    }
}
