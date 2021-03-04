using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sale_tax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_rate",
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

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_taxable_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_3_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_3_taxable_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "taxable_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tax_1_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_1_rate",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_1_taxable_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_2_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_2_rate",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_2_taxable_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_3_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_3_rate",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_3_taxable_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "taxable_amount",
                table: "tbl_sale");
        }
    }
}
