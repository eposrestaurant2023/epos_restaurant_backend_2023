using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_printer_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "group_item_type_id",
                table: "tbl_sale_product");

            migrationBuilder.AddColumn<int>(
                name: "group_item_type_id",
                table: "tbl_sale_product_print_queue",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "group_item_type_id",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.AddColumn<int>(
                name: "group_item_type_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
