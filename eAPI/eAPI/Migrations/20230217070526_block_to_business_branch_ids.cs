using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class block_to_business_branch_ids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "block_to_business_branch_ids",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "block_to_business_branch_ids",
                table: "tbl_product");
        }
    }
}
