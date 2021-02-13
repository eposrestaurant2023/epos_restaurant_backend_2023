using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_category_0393 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_bult_in",
                table: "tbl_product_group",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_ingredient_category",
                table: "tbl_product_category",
                type: "bit",
                nullable: false,
                defaultValue: false);
             
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_bult_in",
                table: "tbl_product_group");

            migrationBuilder.DropColumn(
                name: "is_ingredient_category",
                table: "tbl_product_category"); 
        }
    }
}
