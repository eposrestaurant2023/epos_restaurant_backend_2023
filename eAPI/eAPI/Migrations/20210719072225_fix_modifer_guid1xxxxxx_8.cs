using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_modifer_guid1xxxxxx_8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("tbl_modifier_group_item");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
