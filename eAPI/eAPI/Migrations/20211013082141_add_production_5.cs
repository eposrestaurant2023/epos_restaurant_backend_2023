using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_production_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "group_item_type_id",
                table: "tbl_product_printer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "group_item_type_id",
                table: "tbl_printer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "group_item_type_id",
                table: "tbl_product_printer");

            migrationBuilder.DropColumn(
                name: "group_item_type_id",
                table: "tbl_printer");
        }
    }
}
