using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_tbl_file : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "purchase_order_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vendor_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "purchase_order_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vendor_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_vendor_id",
                table: "tbl_history",
                column: "vendor_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_purchase_order_id",
                table: "tbl_attach_files",
                column: "purchase_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_vendor_id",
                table: "tbl_attach_files",
                column: "vendor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_vendor_purchase_order_id",
                table: "tbl_attach_files",
                column: "purchase_order_id",
                principalTable: "tbl_vendor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_vendor_vendor_id",
                table: "tbl_attach_files",
                column: "vendor_id",
                principalTable: "tbl_vendor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_vendor_vendor_id",
                table: "tbl_history",
                column: "vendor_id",
                principalTable: "tbl_vendor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_vendor_purchase_order_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_vendor_vendor_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_vendor_vendor_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_vendor_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_purchase_order_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_vendor_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "purchase_order_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "vendor_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "purchase_order_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "vendor_id",
                table: "tbl_attach_files");
        }
    }
}
