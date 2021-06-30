using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_taxt_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "total_revenue",
                table: "tbl_sale_product",
                newName: "profit");

            migrationBuilder.RenameColumn(
                name: "total_discount",
                table: "tbl_sale_product",
                newName: "net_sale");

            migrationBuilder.RenameColumn(
                name: "taxable_amount",
                table: "tbl_sale_product",
                newName: "discount_amount");

            migrationBuilder.AddColumn<decimal>(
                name: "discount_amount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_net_sale",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_profit",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount_amount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "total_net_sale",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "total_profit",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "profit",
                table: "tbl_sale_product",
                newName: "total_revenue");

            migrationBuilder.RenameColumn(
                name: "net_sale",
                table: "tbl_sale_product",
                newName: "total_discount");

            migrationBuilder.RenameColumn(
                name: "discount_amount",
                table: "tbl_sale_product",
                newName: "taxable_amount");
        }
    }
}
