using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class kkk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "modifier_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_modifier_id",
                table: "tbl_history",
                column: "modifier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_modifier_modifier_id",
                table: "tbl_history",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_modifier_modifier_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_kitchen_group_kitchen_group_id",
                table: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_kitchen_group");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_kitchen_group_id",
                table: "tbl_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_modifier_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "initial_quantity",
                table: "tbl_stock_location_product");

            migrationBuilder.DropColumn(
                name: "is_product_has_inventory_transaction",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "kitchen_group_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "tbl_menu");

            migrationBuilder.DropColumn(
                name: "modifier_id",
                table: "tbl_history");
        }
    }
}
