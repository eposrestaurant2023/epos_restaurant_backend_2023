using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_predefine_discount_codex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discount_name",
                table: "tbl_predefine_discount_code",
                newName: "discount_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discount_code",
                table: "tbl_predefine_discount_code",
                newName: "discount_name");
        }
    }
}
