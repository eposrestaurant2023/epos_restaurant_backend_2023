using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_product_add_vendor_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "vendor_id",
                table: "tbl_product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_vendor_id",
                table: "tbl_product",
                column: "vendor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_vendor_vendor_id",
                table: "tbl_product",
                column: "vendor_id",
                principalTable: "tbl_vendor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_vendor_vendor_id",
                table: "tbl_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_vendor_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "vendor_id",
                table: "tbl_product");
        }
    }
}
