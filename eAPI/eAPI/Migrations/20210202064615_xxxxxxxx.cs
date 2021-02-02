using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class xxxxxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_product_id",
                table: "tbl_attach_files",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_product_product_id",
                table: "tbl_attach_files",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_product_product_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_product_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "product_id",
                table: "tbl_attach_files");
        }
    }
}
