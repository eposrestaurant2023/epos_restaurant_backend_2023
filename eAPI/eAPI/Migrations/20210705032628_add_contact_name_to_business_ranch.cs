using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_contact_name_to_business_ranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             
            migrationBuilder.AddColumn<string>(
                name: "contact_name",
                table: "tbl_business_branch",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

              
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_product_tax");

            migrationBuilder.DropColumn(
                name: "product_tax_value",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "contact_name",
                table: "tbl_business_branch");

            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_product_category",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_product_category",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_product_category",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
