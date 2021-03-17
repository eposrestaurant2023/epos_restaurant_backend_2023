using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class sale_status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_status_id",
                table: "tbl_sale",
                column: "status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_sale_status_status_id",
                table: "tbl_sale",
                column: "status_id",
                principalTable: "tbl_sale_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_sale_status_status_id",
                table: "tbl_sale");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_status_id",
                table: "tbl_sale");
        }
    }
}
