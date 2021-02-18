using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_unitxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_unit_unit_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_product");

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
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_unit_unit_id",
                table: "tbl_product");

            migrationBuilder.AlterColumn<int>(
                name: "unit_id",
                table: "tbl_product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_unit_unit_id",
                table: "tbl_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
