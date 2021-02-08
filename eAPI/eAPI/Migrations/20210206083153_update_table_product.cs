using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_table_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "vendor_note",
                table: "tbl_purchase_order",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "tbl_product",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "is_ingredient_product",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_menu_product",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "is_ingredient_product",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "is_menu_product",
                table: "tbl_product");

            migrationBuilder.AlterColumn<string>(
                name: "vendor_note",
                table: "tbl_purchase_order",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");
        }
    }
}
