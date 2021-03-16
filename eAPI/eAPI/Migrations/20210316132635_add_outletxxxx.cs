using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_outletxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_cashier_shift");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_working_day",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_cashier_shift",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
