using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class station_disd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount_lable",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "is_free",
                table: "tbl_sale_product");

            migrationBuilder.RenameColumn(
                name: "free_note",
                table: "tbl_sale_product",
                newName: "discount_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discount_code",
                table: "tbl_sale_product",
                newName: "free_note");

            migrationBuilder.AddColumn<string>(
                name: "discount_lable",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<bool>(
                name: "is_free",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
