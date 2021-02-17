using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_unitx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_unit_id",
                table: "tbl_product",
                column: "unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_unit_unit_id",
                table: "tbl_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_unit_unit_id",
                table: "tbl_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_unit_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_product");
        }
    }
}
