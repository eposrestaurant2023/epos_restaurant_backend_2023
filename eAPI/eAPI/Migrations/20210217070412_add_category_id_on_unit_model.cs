using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_category_id_on_unit_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "unit_category_id",
                table: "tbl_unit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "unit_categoryid",
                table: "tbl_unit",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_unit_unit_categoryid",
                table: "tbl_unit",
                column: "unit_categoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_unit_categoryid",
                table: "tbl_unit",
                column: "unit_categoryid",
                principalTable: "tbl_unit_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_unit_tbl_unit_category_unit_categoryid",
                table: "tbl_unit");

            migrationBuilder.DropIndex(
                name: "IX_tbl_unit_unit_categoryid",
                table: "tbl_unit");

            migrationBuilder.DropColumn(
                name: "unit_category_id",
                table: "tbl_unit");

            migrationBuilder.DropColumn(
                name: "unit_categoryid",
                table: "tbl_unit");
        }
    }
}
