using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sale_update_tax_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "discount_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_1_taxable_amount",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "discount_type",
                table: "tbl_sale_product",
                newName: "sale_product_discount_type");

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
                newName: "total_tax_amount");

            migrationBuilder.RenameColumn(
                name: "taxable_amount",
                table: "tbl_sale",
                newName: "total_discount_amount");

            migrationBuilder.RenameColumn(
                name: "tax_3_taxable_amount",
                table: "tbl_sale",
                newName: "sale_discount_value");

            migrationBuilder.RenameColumn(
                name: "tax_2_taxable_amount",
                table: "tbl_sale",
                newName: "sale_discount_amount");

            migrationBuilder.RenameColumn(
                name: "discount_type",
                table: "tbl_sale",
                newName: "sale_discount_type");

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
                name: "sale_product_discount_type",
                table: "tbl_sale_product",
                newName: "discount_type");

            migrationBuilder.RenameColumn(
                name: "total_tax_amount",
                table: "tbl_sale",
                newName: "total_discount");

            migrationBuilder.RenameColumn(
                name: "total_discount_amount",
                table: "tbl_sale",
                newName: "taxable_amount");

            migrationBuilder.RenameColumn(
                name: "sale_discount_value",
                table: "tbl_sale",
                newName: "tax_3_taxable_amount");

            migrationBuilder.RenameColumn(
                name: "sale_discount_type",
                table: "tbl_sale",
                newName: "discount_type");

            migrationBuilder.RenameColumn(
                name: "sale_discount_amount",
                table: "tbl_sale",
                newName: "tax_2_taxable_amount");

            migrationBuilder.AddColumn<decimal>(
                name: "discount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "discount_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_taxable_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
