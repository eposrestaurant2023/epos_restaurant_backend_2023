using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_table_noted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "changed_table_note",
                table: "tbl_sale",
                newName: "changed_table_data");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "changed_table_data",
                table: "tbl_sale",
                newName: "changed_table_note");
        }
    }
}
