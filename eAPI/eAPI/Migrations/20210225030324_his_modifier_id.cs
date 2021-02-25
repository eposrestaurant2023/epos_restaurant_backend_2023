using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class his_modifier_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "modifier_group_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_modifier_group_id",
                table: "tbl_history",
                column: "modifier_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_modifier_group_modifier_group_id",
                table: "tbl_history",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_modifier_group_modifier_group_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_modifier_group_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_history");
        }
    }
}
