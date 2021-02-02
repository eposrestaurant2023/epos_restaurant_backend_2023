using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_dbxxxxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_product_menu",
                table: "tbl_product_menu");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "tbl_product_menu",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_product_menu",
                table: "tbl_product_menu",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_menu_product_id",
                table: "tbl_product_menu",
                column: "product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_product_menu",
                table: "tbl_product_menu");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_menu_product_id",
                table: "tbl_product_menu");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tbl_product_menu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_product_menu",
                table: "tbl_product_menu",
                columns: new[] { "product_id", "menu_id" });
        }
    }
}
