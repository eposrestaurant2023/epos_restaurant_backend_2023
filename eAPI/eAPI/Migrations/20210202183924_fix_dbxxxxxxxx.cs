using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_dbxxxxxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "background_color",
                table: "tbl_menu",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "photo",
                table: "tbl_menu",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "text_color",
                table: "tbl_menu",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "background_color",
                table: "tbl_menu");

            migrationBuilder.DropColumn(
                name: "photo",
                table: "tbl_menu");

            migrationBuilder.DropColumn(
                name: "text_color",
                table: "tbl_menu");
        }
    }
}
