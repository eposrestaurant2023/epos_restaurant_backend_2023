using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_cat_id_into_unit_table_090000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_unit_category_id",
                table: "tbl_unit");

            migrationBuilder.AlterColumn<int>(
                name: "unit_category_id",
                table: "tbl_unit",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_unit_category_id",
                table: "tbl_unit",
                column: "unit_category_id",
                principalTable: "tbl_unit_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_unit_category_id",
                table: "tbl_unit");

            migrationBuilder.AlterColumn<int>(
                name: "unit_category_id",
                table: "tbl_unit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_unit_category_id",
                table: "tbl_unit",
                column: "unit_category_id",
                principalTable: "tbl_unit_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
