using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sale_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_sale_use_tax",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_sale_use_tax",
                table: "tbl_sale",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_sale_use_tax",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "is_sale_use_tax",
                table: "tbl_sale");
        }
    }
}
