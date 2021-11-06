using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_sync_working_day : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_synced",
                table: "tbl_working_day",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_synced",
                table: "tbl_cashier_shift",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_synced",
                table: "tbl_cash_drawer_amount",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_synced",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "is_synced",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "is_synced",
                table: "tbl_cash_drawer_amount");
        }
    }
}
