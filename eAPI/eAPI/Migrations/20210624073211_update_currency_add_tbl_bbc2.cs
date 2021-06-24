using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_currency_add_tbl_bbc2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_currency_currency_id",
                table: "tbl_business_branch_currency",
                column: "currency_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_currency_tbl_business_branch_business_branch_id",
                table: "tbl_business_branch_currency",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_currency_tbl_currency_currency_id",
                table: "tbl_business_branch_currency",
                column: "currency_id",
                principalTable: "tbl_currency",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_currency_tbl_business_branch_business_branch_id",
                table: "tbl_business_branch_currency");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_currency_tbl_currency_currency_id",
                table: "tbl_business_branch_currency");

            migrationBuilder.DropIndex(
                name: "IX_tbl_business_branch_currency_currency_id",
                table: "tbl_business_branch_currency");
        }
    }
}
