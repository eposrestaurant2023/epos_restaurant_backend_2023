using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_table_name_tyo_que : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "old_table_name",
                table: "tbl_sale_product_print_queue",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "table_name",
                table: "tbl_sale_product_print_queue",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "old_table_name",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.DropColumn(
                name: "table_name",
                table: "tbl_sale_product_print_queue");
        }
    }
}
