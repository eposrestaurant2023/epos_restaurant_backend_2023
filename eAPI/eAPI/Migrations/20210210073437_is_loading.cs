using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class is_loading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_change_status",
                table: "tbl_business_branch_payment_type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_loading",
                table: "tbl_business_branch_payment_type",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_change_status",
                table: "tbl_business_branch_payment_type");

            migrationBuilder.DropColumn(
                name: "is_loading",
                table: "tbl_business_branch_payment_type");
        }
    }
}
