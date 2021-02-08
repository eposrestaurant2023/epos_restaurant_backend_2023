using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_add_product_type_032 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_product_type_product_type_id",
                table: "tbl_product");

            migrationBuilder.AlterColumn<int>(
                name: "product_type_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_product_type_product_type_id",
                table: "tbl_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_product_type_product_type_id",
                table: "tbl_product");

            migrationBuilder.AlterColumn<int>(
                name: "product_type_id",
                table: "tbl_product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_product_type_product_type_id",
                table: "tbl_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
