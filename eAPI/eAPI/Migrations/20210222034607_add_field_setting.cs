using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_field_setting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "group_name",
                table: "tbl_setting",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<bool>(
                name: "is_public",
                table: "tbl_setting",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "setting_value",
                table: "tbl_setting",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "tbl_setting",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "group_name",
                table: "tbl_setting");

            migrationBuilder.DropColumn(
                name: "is_public",
                table: "tbl_setting");

            migrationBuilder.DropColumn(
                name: "setting_value",
                table: "tbl_setting");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "tbl_setting");
        }
    }
}
