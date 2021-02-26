using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_multiple_select",
                table: "tbl_modifier_group_item",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_required",
                table: "tbl_modifier_group_item",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "section_name",
                table: "tbl_modifier_group_item",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_multiple_select",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "is_required",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "section_name",
                table: "tbl_modifier_group_item");
        }
    }
}
