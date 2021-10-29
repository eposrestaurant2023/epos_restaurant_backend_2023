using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class ad_portion_productioon_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_portion_id",
                table: "tbl_production_product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_production_product_product_portion_id",
                table: "tbl_production_product",
                column: "product_portion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_production_product_tbl_product_portion_product_portion_id",
                table: "tbl_production_product",
                column: "product_portion_id",
                principalTable: "tbl_product_portion",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_production_product_tbl_product_portion_product_portion_id",
                table: "tbl_production_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_production_product_product_portion_id",
                table: "tbl_production_product");

            migrationBuilder.DropColumn(
                name: "product_portion_id",
                table: "tbl_production_product");
        }
    }
}
