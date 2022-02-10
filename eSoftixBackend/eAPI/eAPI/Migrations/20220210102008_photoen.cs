using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class photoen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "photo",
                table: "tbl_eknowledge_base",
                newName: "photo_kh");

            migrationBuilder.AddColumn<string>(
                name: "photo_en",
                table: "tbl_eknowledge_base",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photo_en",
                table: "tbl_eknowledge_base");

            migrationBuilder.RenameColumn(
                name: "photo_kh",
                table: "tbl_eknowledge_base",
                newName: "photo");
        }
    }
}
