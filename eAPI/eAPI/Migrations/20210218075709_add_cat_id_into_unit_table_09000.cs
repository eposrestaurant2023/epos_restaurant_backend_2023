using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_cat_id_into_unit_table_09000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_UnitCategoryModelid",
                table: "tbl_unit");

            migrationBuilder.RenameColumn(
                name: "UnitCategoryModelid",
                table: "tbl_unit",
                newName: "unit_category_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_unit_UnitCategoryModelid",
                table: "tbl_unit",
                newName: "IX_tbl_unit_unit_category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_unit_category_id",
                table: "tbl_unit",
                column: "unit_category_id",
                principalTable: "tbl_unit_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_unit_category_id",
                table: "tbl_unit");

            migrationBuilder.RenameColumn(
                name: "unit_category_id",
                table: "tbl_unit",
                newName: "UnitCategoryModelid");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_unit_unit_category_id",
                table: "tbl_unit",
                newName: "IX_tbl_unit_UnitCategoryModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_UnitCategoryModelid",
                table: "tbl_unit",
                column: "UnitCategoryModelid",
                principalTable: "tbl_unit_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
