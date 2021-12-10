using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class shaptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "shape",
                table: "tbl_table",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shape",
                table: "tbl_table");

            migrationBuilder.DropColumn(
                name: "customer_group_name",
                table: "tbl_customer");
        }
    }
}
