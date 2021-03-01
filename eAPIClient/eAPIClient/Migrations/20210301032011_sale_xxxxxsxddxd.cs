using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sale_xxxxxsxddxd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_product_id",
                table: "tbl_product_modifier");

            migrationBuilder.AlterColumn<int>(
                name: "product_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_product_id",
                table: "tbl_product_modifier",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_product_id",
                table: "tbl_product_modifier");

            migrationBuilder.AlterColumn<int>(
                name: "product_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_product_id",
                table: "tbl_product_modifier",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
