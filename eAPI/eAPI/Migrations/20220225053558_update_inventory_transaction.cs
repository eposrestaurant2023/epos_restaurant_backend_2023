using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_inventory_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_inventory_check_inventory_check_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_production_production_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_transaction_inventory_check_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_transaction_production_id",
                table: "tbl_inventory_transaction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_inventory_check_id",
                table: "tbl_inventory_transaction",
                column: "inventory_check_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_production_id",
                table: "tbl_inventory_transaction",
                column: "production_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_inventory_check_inventory_check_id",
                table: "tbl_inventory_transaction",
                column: "inventory_check_id",
                principalTable: "tbl_inventory_check",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_production_production_id",
                table: "tbl_inventory_transaction",
                column: "production_id",
                principalTable: "tbl_production",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
