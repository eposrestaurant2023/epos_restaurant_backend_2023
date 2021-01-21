using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_customer_groupxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_customer_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id",
                principalTable: "tbl_customer_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_customer_customer_group_id",
                table: "tbl_customer");
        }
    }
}
