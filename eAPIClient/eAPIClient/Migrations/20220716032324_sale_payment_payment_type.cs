using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sale_payment_payment_type : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payment_fee",
                table: "tbl_sale_payment");

            migrationBuilder.DropColumn(
                name: "payment_fee_amount",
                table: "tbl_sale_payment");
        }
    }
}
