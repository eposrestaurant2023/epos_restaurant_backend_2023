using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_price_rule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "price_rule_id",
                table: "tbl_table",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_table_price_rule_id",
                table: "tbl_table",
                column: "price_rule_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_table_tbl_price_rule_price_rule_id",
                table: "tbl_table",
                column: "price_rule_id",
                principalTable: "tbl_price_rule",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_table_tbl_price_rule_price_rule_id",
                table: "tbl_table");

            migrationBuilder.DropIndex(
                name: "IX_tbl_table_price_rule_id",
                table: "tbl_table");

            migrationBuilder.DropColumn(
                name: "price_rule_id",
                table: "tbl_table");
        }
    }
}
