using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_park_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "park_sale_product_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "redeem_sale_product_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "park_sale_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "redeem_park_sale_id",
                table: "tbl_sale");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "park_sale_product_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "redeem_sale_product_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "park_sale_id",
                table: "tbl_sale",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "redeem_park_sale_id",
                table: "tbl_sale",
                type: "int",
                nullable: true);
        }
    }
}
