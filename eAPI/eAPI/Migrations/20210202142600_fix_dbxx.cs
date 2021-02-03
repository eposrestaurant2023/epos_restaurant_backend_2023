using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_dbxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_id",
                table: "tbl_product_portion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_portion_product_id",
                table: "tbl_product_portion",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_portion_tbl_product_product_id",
                table: "tbl_product_portion",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_portion_tbl_product_product_id",
                table: "tbl_product_portion");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_portion_product_id",
                table: "tbl_product_portion");

            migrationBuilder.DropColumn(
                name: "product_id",
                table: "tbl_product_portion");
        }
    }
}
