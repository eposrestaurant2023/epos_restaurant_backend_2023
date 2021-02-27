using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class kkk_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "modifier_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_modifier_id",
                table: "tbl_attach_files",
                column: "modifier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_modifier_id",
                table: "tbl_attach_files",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_modifier_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_modifier_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "modifier_id",
                table: "tbl_attach_files");
        }
    }
}
