using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tax_1_name",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_2_name",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_3_name",
                table: "tbl_station");

            migrationBuilder.RenameColumn(
                name: "discount_amount",
                table: "tbl_sale_product",
                newName: "total_discount_amount");

            migrationBuilder.RenameColumn(
                name: "discount",
                table: "tbl_sale_product",
                newName: "sale_product_discount_value");

            migrationBuilder.RenameColumn(
                name: "total_discount",
                table: "tbl_sale",
                newName: "total_discount_amount");

            migrationBuilder.RenameColumn(
                name: "discount_amount",
                table: "tbl_sale",
                newName: "sale_discount_value");

            migrationBuilder.RenameColumn(
                name: "discount",
                table: "tbl_sale",
                newName: "sale_discount_amount");

            migrationBuilder.AddColumn<decimal>(
                name: "sale_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "sale_discount_value",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "sale_product_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sale_discount_amount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "sale_discount_value",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "sale_product_discount_amount",
                table: "tbl_sale_product");

            migrationBuilder.RenameColumn(
                name: "total_discount_amount",
                table: "tbl_sale_product",
                newName: "discount_amount");

            migrationBuilder.RenameColumn(
                name: "sale_product_discount_value",
                table: "tbl_sale_product",
                newName: "discount");

            migrationBuilder.RenameColumn(
                name: "total_discount_amount",
                table: "tbl_sale",
                newName: "total_discount");

            migrationBuilder.RenameColumn(
                name: "sale_discount_value",
                table: "tbl_sale",
                newName: "discount_amount");

            migrationBuilder.RenameColumn(
                name: "sale_discount_amount",
                table: "tbl_sale",
                newName: "discount");

            migrationBuilder.AddColumn<string>(
                name: "tax_1_name",
                table: "tbl_station",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "tax_2_name",
                table: "tbl_station",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "tax_3_name",
                table: "tbl_station",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }
    }
}
