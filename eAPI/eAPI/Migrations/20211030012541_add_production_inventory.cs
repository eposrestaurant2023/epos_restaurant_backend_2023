using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_production_inventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "production_id",
                table: "tbl_inventory_transaction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_production_id",
                table: "tbl_inventory_transaction",
                column: "production_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_production_production_id",
                table: "tbl_inventory_transaction",
                column: "production_id",
                principalTable: "tbl_production",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_production_production_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_transaction_production_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "production_id",
                table: "tbl_inventory_transaction");
        }
    }
}
