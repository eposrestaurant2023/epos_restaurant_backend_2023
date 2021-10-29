using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class ad_portion_productioon_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_production_product_tbl_product_portion_product_portion_id",
                table: "tbl_production_product");

            migrationBuilder.AlterColumn<int>(
                name: "product_portion_id",
                table: "tbl_production_product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_production_product_tbl_product_portion_product_portion_id",
                table: "tbl_production_product",
                column: "product_portion_id",
                principalTable: "tbl_product_portion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_production_product_tbl_product_portion_product_portion_id",
                table: "tbl_production_product");

            migrationBuilder.AlterColumn<int>(
                name: "product_portion_id",
                table: "tbl_production_product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_production_product_tbl_product_portion_product_portion_id",
                table: "tbl_production_product",
                column: "product_portion_id",
                principalTable: "tbl_product_portion",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
