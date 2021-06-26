using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class dbdx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "product_group_name_kh",
                table: "tbl_product",
                newName: "product_group_kh");

            migrationBuilder.RenameColumn(
                name: "product_group_name_en",
                table: "tbl_product",
                newName: "product_group_en");

            migrationBuilder.RenameColumn(
                name: "product_category_name_kh",
                table: "tbl_product",
                newName: "product_category_kh");

            migrationBuilder.RenameColumn(
                name: "product_category_name_en",
                table: "tbl_product",
                newName: "product_category_en");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "product_group_kh",
                table: "tbl_product",
                newName: "product_group_name_kh");

            migrationBuilder.RenameColumn(
                name: "product_group_en",
                table: "tbl_product",
                newName: "product_group_name_en");

            migrationBuilder.RenameColumn(
                name: "product_category_kh",
                table: "tbl_product",
                newName: "product_category_name_kh");

            migrationBuilder.RenameColumn(
                name: "product_category_en",
                table: "tbl_product",
                newName: "product_category_name_en");
        }
    }
}
