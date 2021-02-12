using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_attach_files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stock_transfer_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_stock_transfer_id",
                table: "tbl_attach_files",
                column: "stock_transfer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_stock_transfer_stock_transfer_id",
                table: "tbl_attach_files",
                column: "stock_transfer_id",
                principalTable: "tbl_stock_transfer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_stock_transfer_stock_transfer_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_stock_transfer_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "stock_transfer_id",
                table: "tbl_attach_files");
        }
    }
}
