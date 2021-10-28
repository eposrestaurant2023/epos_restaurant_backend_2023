using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class currenct_1dxxxxz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "multiplier",
                table: "tbl_sale_product_status",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_sale_product_status");
        }
    }
}
