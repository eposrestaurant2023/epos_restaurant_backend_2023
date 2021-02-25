using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class attach_file_modifier_group_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "modifier_group_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_modifier_group_id",
                table: "tbl_attach_files",
                column: "modifier_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_group_modifier_group_id",
                table: "tbl_attach_files",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_group_modifier_group_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_modifier_group_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_attach_files");
        }
    }
}
