using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class attproduction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "production_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_production_id",
                table: "tbl_attach_files",
                column: "production_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_production_production_id",
                table: "tbl_attach_files",
                column: "production_id",
                principalTable: "tbl_production",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_production_production_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_production_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "production_id",
                table: "tbl_attach_files");
        }
    }
}
