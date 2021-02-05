using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_files_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_sale_SaleModelid",
                table: "tbl_history");

            migrationBuilder.RenameColumn(
                name: "SaleModelid",
                table: "tbl_history",
                newName: "sale_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_history_SaleModelid",
                table: "tbl_history",
                newName: "IX_tbl_history_sale_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_sale_sale_id",
                table: "tbl_history",
                column: "sale_id",
                principalTable: "tbl_sale",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_sale_sale_id",
                table: "tbl_history");

            migrationBuilder.RenameColumn(
                name: "sale_id",
                table: "tbl_history",
                newName: "SaleModelid");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_history_sale_id",
                table: "tbl_history",
                newName: "IX_tbl_history_SaleModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_sale_SaleModelid",
                table: "tbl_history",
                column: "SaleModelid",
                principalTable: "tbl_sale",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
