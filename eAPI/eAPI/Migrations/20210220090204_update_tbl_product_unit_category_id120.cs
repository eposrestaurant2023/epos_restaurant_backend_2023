using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_unit_category_id120 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_unit_category_unit_category_id",
                table: "tbl_product");

            migrationBuilder.AlterColumn<int>(
                name: "unit_category_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_unit_category_unit_category_id",
                table: "tbl_product",
                column: "unit_category_id",
                principalTable: "tbl_unit_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_unit_category_unit_category_id",
                table: "tbl_product");

            migrationBuilder.AlterColumn<int>(
                name: "unit_category_id",
                table: "tbl_product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_unit_category_unit_category_id",
                table: "tbl_product",
                column: "unit_category_id",
                principalTable: "tbl_unit_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
