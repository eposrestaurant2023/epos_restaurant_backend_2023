using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class disc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "default_discount_percentage",
                table: "tbl_table");

            migrationBuilder.RenameColumn(
                name: "discount_percentage",
                table: "tbl_discount_promotion_item",
                newName: "discount_value");

            migrationBuilder.AddColumn<string>(
                name: "discount_type",
                table: "tbl_table",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "discount_value",
                table: "tbl_table",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "allow_choose",
                table: "tbl_printer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "discount_type",
                table: "tbl_discount_promotion_item",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount_type",
                table: "tbl_table");

            migrationBuilder.DropColumn(
                name: "discount_value",
                table: "tbl_table");

            migrationBuilder.DropColumn(
                name: "allow_choose",
                table: "tbl_printer");

            migrationBuilder.DropColumn(
                name: "discount_type",
                table: "tbl_discount_promotion_item");

            migrationBuilder.RenameColumn(
                name: "discount_value",
                table: "tbl_discount_promotion_item",
                newName: "discount_percentage");

            migrationBuilder.AddColumn<double>(
                name: "default_discount_percentage",
                table: "tbl_table",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
