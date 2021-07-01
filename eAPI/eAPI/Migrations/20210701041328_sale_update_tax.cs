using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class sale_update_tax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tax_1_taxable_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_2_taxable_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "tax_3_taxable_amount",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "taxable_amount",
                table: "tbl_sale",
                newName: "total_tax_amount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "total_tax_amount",
                table: "tbl_sale",
                newName: "taxable_amount");

            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_taxable_amount",
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
                name: "tax_3_taxable_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
