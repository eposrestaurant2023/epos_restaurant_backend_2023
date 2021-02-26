using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class modifier_ingredients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "tbl_modifier_ingredient",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "tbl_modifier_ingredient",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "total_cost",
                table: "tbl_modifier_ingredient",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_modifier_ingredient",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_ingredient_unit_id",
                table: "tbl_modifier_ingredient",
                column: "unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_ingredient_tbl_unit_unit_id",
                table: "tbl_modifier_ingredient",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_ingredient_tbl_unit_unit_id",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_ingredient_unit_id",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropColumn(
                name: "cost",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropColumn(
                name: "total_cost",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_modifier_ingredient");
        }
    }
}
