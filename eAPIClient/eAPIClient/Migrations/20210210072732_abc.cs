using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class abc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_type_tbl_currency_currency_id",
                table: "tbl_payment_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_currency",
                table: "tbl_currency");


            migrationBuilder.AddColumn<int>(
                name: "currencyid",
                table: "tbl_currency",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_currency",
                table: "tbl_currency",
                column: "currencyid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_type_tbl_currency_currency_id",
                table: "tbl_payment_type",
                column: "currency_id",
                principalTable: "tbl_currency",
                principalColumn: "currencyid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_type_tbl_currency_currency_id",
                table: "tbl_payment_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_currency",
                table: "tbl_currency");

            migrationBuilder.DropColumn(
                name: "currencyid",
                table: "tbl_currency");


            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_currency",
                table: "tbl_currency",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_type_tbl_currency_currency_id",
                table: "tbl_payment_type",
                column: "currency_id",
                principalTable: "tbl_currency",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
