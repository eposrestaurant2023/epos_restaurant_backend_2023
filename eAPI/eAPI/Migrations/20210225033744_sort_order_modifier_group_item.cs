using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class sort_order_modifier_group_item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "tbl_modifier");

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "tbl_modifier_group_item");

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "tbl_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
