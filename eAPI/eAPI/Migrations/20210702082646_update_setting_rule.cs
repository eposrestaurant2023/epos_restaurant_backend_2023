using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_setting_rule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "permission_options",
                table: "tbl_setting",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_permission_option",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "permission_options",
                table: "tbl_setting");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_permission_option");
        }
    }
}
