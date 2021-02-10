using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class is_loading_price : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_change_status",
                table: "tbl_business_branch_price_rule",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_loading",
                table: "tbl_business_branch_price_rule",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_change_status",
                table: "tbl_business_branch_price_rule");

            migrationBuilder.DropColumn(
                name: "is_loading",
                table: "tbl_business_branch_price_rule");
        }
    }
}
