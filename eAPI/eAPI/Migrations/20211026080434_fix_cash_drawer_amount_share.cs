using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_cash_drawer_amount_share : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_cash_drawer_amount_working_day_id",
                table: "tbl_cash_drawer_amount",
                column: "working_day_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cash_drawer_amount_tbl_working_day_working_day_id",
                table: "tbl_cash_drawer_amount",
                column: "working_day_id",
                principalTable: "tbl_working_day",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cash_drawer_amount_tbl_working_day_working_day_id",
                table: "tbl_cash_drawer_amount");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cash_drawer_amount_working_day_id",
                table: "tbl_cash_drawer_amount");
        }
    }
}
