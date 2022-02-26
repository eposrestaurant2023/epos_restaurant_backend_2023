using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_amount_inventory_check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "actual_quantity",
                table: "tbl_inventory_check_product",
                type: "decimal(19,8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");

            migrationBuilder.AddColumn<decimal>(
                name: "actual_amount",
                table: "tbl_inventory_check_product",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "expected_amount",
                table: "tbl_inventory_check_product",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actual_amount",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "expected_amount",
                table: "tbl_inventory_check_product");

            migrationBuilder.AlterColumn<decimal>(
                name: "actual_quantity",
                table: "tbl_inventory_check_product",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)",
                oldNullable: true);
        }
    }
}
