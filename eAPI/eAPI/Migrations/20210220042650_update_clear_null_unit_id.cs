using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_clear_null_unit_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_unit_unit_id",
                table: "tbl_product");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_portion_tbl_unit_unit_id",
                table: "tbl_product_portion");

            migrationBuilder.AlterColumn<int>(
                name: "unit_id",
                table: "tbl_product_portion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "unit_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_unit_unit_id",
                table: "tbl_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_portion_tbl_unit_unit_id",
                table: "tbl_product_portion",
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

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_portion_tbl_unit_unit_id",
                table: "tbl_product_portion");

            migrationBuilder.AlterColumn<int>(
                name: "unit_id",
                table: "tbl_product_portion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "unit_id",
                table: "tbl_product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_unit_unit_id",
                table: "tbl_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_portion_tbl_unit_unit_id",
                table: "tbl_product_portion",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
