using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_cusomer_model_add_is_schnch : Migration
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
