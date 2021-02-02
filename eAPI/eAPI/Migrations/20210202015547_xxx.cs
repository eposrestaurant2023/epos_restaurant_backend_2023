using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class xxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "image_name",
                table: "tbl_product",
                newName: "photo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "photo",
                table: "tbl_product",
                newName: "image_name");
        }
    }
}
