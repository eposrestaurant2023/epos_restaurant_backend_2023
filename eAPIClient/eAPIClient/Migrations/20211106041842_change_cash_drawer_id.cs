using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class change_cash_drawer_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "outlet_id",
                table: "tbl_document_number",
                newName: "cash_drawer_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cash_drawer_id",
                table: "tbl_document_number",
                newName: "outlet_id");
        }
    }
}
