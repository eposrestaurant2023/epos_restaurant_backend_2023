using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class xxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_product_type_product_type_id",
                table: "tbl_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_product_type_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_type_id",
                table: "tbl_product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_type_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_product_type_id",
                table: "tbl_product",
                column: "product_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_product_type_product_type_id",
                table: "tbl_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
