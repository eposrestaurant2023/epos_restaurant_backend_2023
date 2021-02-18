using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_ingredient_renamex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_ingredient_tbl_product_product_menu_id",
                table: "tbl_product_ingredient");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_ingredient_product_menu_id",
                table: "tbl_product_ingredient");

            migrationBuilder.DropColumn(
                name: "product_menu_id",
                table: "tbl_product_ingredient");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_menu_id",
                table: "tbl_product_ingredient",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_ingredient_product_menu_id",
                table: "tbl_product_ingredient",
                column: "product_menu_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_ingredient_tbl_product_product_menu_id",
                table: "tbl_product_ingredient",
                column: "product_menu_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
