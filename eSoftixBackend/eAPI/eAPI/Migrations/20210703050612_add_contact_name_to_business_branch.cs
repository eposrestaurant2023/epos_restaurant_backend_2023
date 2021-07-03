using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_contact_name_to_business_branch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contact_name",
                table: "tbl_business_branch",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contact_name",
                table: "tbl_business_branch");
        }
    }
}
