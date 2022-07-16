using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_payment_fee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "payment_fee",
                table: "tbl_sale_payment",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "payment_fee_amount",
                table: "tbl_sale_payment",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "manual_payment_fee",
                table: "tbl_payment_type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "payment_fee",
                table: "tbl_payment_type",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payment_fee",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "payment_fee_amount",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "manual_payment_fee",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "payment_fee",
                table: "tbl_payment_type");
        }
    }
}
