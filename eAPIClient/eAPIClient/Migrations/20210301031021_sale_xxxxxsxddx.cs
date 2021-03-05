using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sale_xxxxxsxddx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_multiple_select",
                table: "tbl_product_modifier",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_required",
                table: "tbl_product_modifier",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_section",
                table: "tbl_product_modifier",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "section_name",
                table: "tbl_product_modifier",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier",
                column: "parent_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier",
                column: "parent_id",
                principalTable: "tbl_product_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "is_multiple_select",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "is_required",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "is_section",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "section_name",
                table: "tbl_product_modifier");
        }
    }
}
