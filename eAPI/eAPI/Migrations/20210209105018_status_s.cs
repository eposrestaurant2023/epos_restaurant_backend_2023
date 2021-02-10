using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class status_s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_business_branch_payment_type",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_business_branch_price_rule");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_business_branch_price_rule");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_business_branch_payment_type");
        }
    }
}
