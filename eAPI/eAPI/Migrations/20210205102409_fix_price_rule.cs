using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_price_rule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                table: "tbl_product_price");

            migrationBuilder.DropColumn(
                name: "is_default",
                table: "tbl_price_rule");

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "tbl_business_branch_price_rule",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_default",
                table: "tbl_business_branch_price_rule");

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "tbl_product_price",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "tbl_price_rule",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
