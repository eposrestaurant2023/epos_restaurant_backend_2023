using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class change_table_noteg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "changed_table_data",
                table: "tbl_sale",
                newName: "old_table_name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "old_table_name",
                table: "tbl_sale",
                newName: "changed_table_data");
        }
    }
}
