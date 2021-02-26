using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class modifier_categoryx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_category_modifier_category_id",
                table: "tbl_modifier");

            migrationBuilder.AlterColumn<int>(
                name: "modifier_category_id",
                table: "tbl_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_category_modifier_category_id",
                table: "tbl_modifier",
                column: "modifier_category_id",
                principalTable: "tbl_modifier_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_category_modifier_category_id",
                table: "tbl_modifier");

            migrationBuilder.AlterColumn<int>(
                name: "modifier_category_id",
                table: "tbl_modifier",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_category_modifier_category_id",
                table: "tbl_modifier",
                column: "modifier_category_id",
                principalTable: "tbl_modifier_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
