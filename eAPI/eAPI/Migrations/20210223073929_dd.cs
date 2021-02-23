using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class dd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "modifier_group_name",
                table: "tbl_modifier_group",
                newName: "modifier_group_name_en");

            migrationBuilder.AddColumn<string>(
                name: "modifier_group_name_kh",
                table: "tbl_modifier_group",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "modifier_group_name_kh",
                table: "tbl_modifier_group");

            migrationBuilder.RenameColumn(
                name: "modifier_group_name_en",
                table: "tbl_modifier_group",
                newName: "modifier_group_name");
        }
    }
}
