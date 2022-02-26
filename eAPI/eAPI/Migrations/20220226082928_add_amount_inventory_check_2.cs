using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_amount_inventory_check_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "actual_amount",
                table: "tbl_inventory_check",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "difference_amount",
                table: "tbl_inventory_check",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "expected_amount",
                table: "tbl_inventory_check",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actual_amount",
                table: "tbl_inventory_check");

            migrationBuilder.DropColumn(
                name: "difference_amount",
                table: "tbl_inventory_check");

            migrationBuilder.DropColumn(
                name: "expected_amount",
                table: "tbl_inventory_check");
        }
    }
}
