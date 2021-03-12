using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class history : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_project_id",
                table: "tbl_history",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_project_id",
                table: "tbl_attach_files",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_project_project_id",
                table: "tbl_attach_files",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_project_project_id",
                table: "tbl_history",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_project_project_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_project_project_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_project_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_project_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "tbl_attach_files");
        }
    }
}
