using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class remove_setting_value_and_is_bus_branch_from_setting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_business_branch",
                table: "tbl_setting");

            migrationBuilder.DropColumn(
                name: "setting_value",
                table: "tbl_setting");

            migrationBuilder.AddColumn<string>(
                name: "setting_value",
                table: "tbl_business_branch_setting",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "setting_value",
                table: "tbl_business_branch_setting");

            migrationBuilder.AddColumn<bool>(
                name: "is_business_branch",
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
        }
    }
}
