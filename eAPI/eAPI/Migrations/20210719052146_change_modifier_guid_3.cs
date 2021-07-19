using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_modifier_guid_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "modifer_group_id",
                table: "tbl_modifier_group_product_category");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "modifier_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_modifier");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "modifier_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "modifier_id",
                table: "tbl_attach_files");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
