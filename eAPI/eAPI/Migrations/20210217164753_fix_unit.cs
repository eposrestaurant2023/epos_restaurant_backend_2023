using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_unit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_product_ingredient");

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_product_ingredient",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_ingredient_unit_id",
                table: "tbl_product_ingredient",
                column: "unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_ingredient_tbl_unit_unit_id",
                table: "tbl_product_ingredient",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_ingredient_tbl_unit_unit_id",
                table: "tbl_product_ingredient");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_ingredient_unit_id",
                table: "tbl_product_ingredient");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_product_ingredient");

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_product_ingredient",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }
    }
}
