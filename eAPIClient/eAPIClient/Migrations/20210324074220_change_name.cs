using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class change_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTypes_CurrencyShareModel_currency_id",
                table: "PaymentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_prefix_price_PaymentTypes_payment_typeid",
                table: "tbl_prefix_price");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentTypes",
                table: "PaymentTypes");

            migrationBuilder.RenameTable(
                name: "PaymentTypes",
                newName: "tbl_payment_type");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTypes_currency_id",
                table: "tbl_payment_type",
                newName: "IX_tbl_payment_type_currency_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_payment_type",
                table: "tbl_payment_type",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_type_CurrencyShareModel_currency_id",
                table: "tbl_payment_type",
                column: "currency_id",
                principalTable: "CurrencyShareModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_prefix_price_tbl_payment_type_payment_typeid",
                table: "tbl_prefix_price",
                column: "payment_typeid",
                principalTable: "tbl_payment_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_type_CurrencyShareModel_currency_id",
                table: "tbl_payment_type");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_prefix_price_tbl_payment_type_payment_typeid",
                table: "tbl_prefix_price");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_payment_type",
                table: "tbl_payment_type");

            migrationBuilder.RenameTable(
                name: "tbl_payment_type",
                newName: "PaymentTypes");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_payment_type_currency_id",
                table: "PaymentTypes",
                newName: "IX_PaymentTypes_currency_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentTypes",
                table: "PaymentTypes",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTypes_CurrencyShareModel_currency_id",
                table: "PaymentTypes",
                column: "currency_id",
                principalTable: "CurrencyShareModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_prefix_price_PaymentTypes_payment_typeid",
                table: "tbl_prefix_price",
                column: "payment_typeid",
                principalTable: "PaymentTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
