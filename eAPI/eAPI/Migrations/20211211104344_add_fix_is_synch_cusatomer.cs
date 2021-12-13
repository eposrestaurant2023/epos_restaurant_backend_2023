using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_fix_is_synch_cusatomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_synced",
                table: "tbl_customer_business_branch");
        }
    }
}
