using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_ingredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_portion_id",
                table: "tbl_product_ingredient",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_ingredient_product_portion_id",
                table: "tbl_product_ingredient",
                column: "product_portion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_ingredient_tbl_product_portion_product_portion_id",
                table: "tbl_product_ingredient",
                column: "product_portion_id",
                principalTable: "tbl_product_portion",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_ingredient_tbl_product_portion_product_portion_id",
                table: "tbl_product_ingredient");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_ingredient_product_portion_id",
                table: "tbl_product_ingredient");

            migrationBuilder.DropColumn(
                name: "product_portion_id",
                table: "tbl_product_ingredient");
        }
    }
}
