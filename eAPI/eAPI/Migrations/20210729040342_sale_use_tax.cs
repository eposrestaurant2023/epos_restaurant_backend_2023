using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class sale_use_tax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_sale_use_tax",
                table: "tbl_sale",
                newName: "is_sale_use_tax_3");

            migrationBuilder.AddColumn<bool>(
                name: "is_sale_use_tax_1",
                table: "tbl_sale",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_sale_use_tax_2",
                table: "tbl_sale",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_sale_use_tax_1",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "is_sale_use_tax_2",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "is_sale_use_tax_3",
                table: "tbl_sale",
                newName: "is_sale_use_tax");
        }
    }
}
