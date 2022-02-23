using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_inventory_check_to_inc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "inventory_check_id",
                table: "tbl_inventory_transaction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_inventory_check_id",
                table: "tbl_inventory_transaction",
                column: "inventory_check_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_production_inventory_check_id",
                table: "tbl_inventory_transaction",
                column: "inventory_check_id",
                principalTable: "tbl_production",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_production_inventory_check_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_transaction_inventory_check_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "inventory_check_id",
                table: "tbl_inventory_transaction");
        }
    }
}
