using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class sale_producxsd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "allow_in_pos_order_list",
                table: "tbl_sale_product_status",
                newName: "allow_display_in_pos_order_list");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "allow_display_in_pos_order_list",
                table: "tbl_sale_product_status",
                newName: "allow_in_pos_order_list");
        }
    }
}
