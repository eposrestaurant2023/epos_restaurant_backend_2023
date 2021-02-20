using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_unit_sale_product_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_sale_product");

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_unit_id",
                table: "tbl_sale_product",
                column: "unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_tbl_unit_unit_id",
                table: "tbl_sale_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_tbl_unit_unit_id",
                table: "tbl_sale_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_product_unit_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_sale_product");

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }
    }
}
