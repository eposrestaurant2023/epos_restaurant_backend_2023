using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class remove_outlet_id_from_tbl_document_number : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_document_number");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_document_number",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
