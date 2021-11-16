using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class motifynotetable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "category_note_id",
                table: "tbl_note",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category_note_id",
                table: "tbl_note");
        }
    }
}
