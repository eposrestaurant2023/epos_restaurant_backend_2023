using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_product_group_name_to_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "product_group_name",
                table: "tbl_product",
                newName: "product_group_name_kh");

            migrationBuilder.AddColumn<string>(
                name: "product_group_name_en",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_group_name_en",
                table: "tbl_product");

            migrationBuilder.RenameColumn(
                name: "product_group_name_kh",
                table: "tbl_product",
                newName: "product_group_name");
        }
    }
}
