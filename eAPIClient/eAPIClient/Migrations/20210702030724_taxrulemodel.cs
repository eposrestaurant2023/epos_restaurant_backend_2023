using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class taxrulemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "reqular_price",
                table: "tbl_sale_product_modifier",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reqular_price",
                table: "tbl_sale_product_modifier");
        }
    }
}
