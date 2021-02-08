using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_add_product_type_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_type_id",
                table: "tbl_product",
                type: "int",
                nullable: true);

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
