using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_category_039344 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_bult_in",
                table: "tbl_product_group",
                newName: "is_built_in");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_built_in",
                table: "tbl_product_group",
                newName: "is_bult_in");
        }
    }
}
