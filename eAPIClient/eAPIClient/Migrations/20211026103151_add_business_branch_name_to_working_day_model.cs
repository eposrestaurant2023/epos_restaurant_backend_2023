using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_business_branch_name_to_working_day_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "business_branch_name_en",
                table: "tbl_working_day",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "business_branch_name_kh",
                table: "tbl_working_day",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "business_branch_name_en",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "business_branch_name_kh",
                table: "tbl_working_day");
        }
    }
}
