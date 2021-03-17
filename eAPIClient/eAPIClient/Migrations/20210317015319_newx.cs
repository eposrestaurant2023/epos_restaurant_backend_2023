using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class newx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_tbl_sale_sale_id",
                table: "tbl_payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_payment",
                table: "tbl_payment");

            migrationBuilder.RenameTable(
                name: "tbl_payment",
                newName: "tbl_sale_payment");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_payment_sale_id",
                table: "tbl_sale_payment",
                newName: "IX_tbl_sale_payment_sale_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_sale_payment",
                table: "tbl_sale_payment",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_payment_tbl_sale_sale_id",
                table: "tbl_sale_payment",
                column: "sale_id",
                principalTable: "tbl_sale",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_payment_tbl_sale_sale_id",
                table: "tbl_sale_payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_sale_payment",
                table: "tbl_sale_payment");

            migrationBuilder.RenameTable(
                name: "tbl_sale_payment",
                newName: "tbl_payment");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_sale_payment_sale_id",
                table: "tbl_payment",
                newName: "IX_tbl_payment_sale_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_payment",
                table: "tbl_payment",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_tbl_sale_sale_id",
                table: "tbl_payment",
                column: "sale_id",
                principalTable: "tbl_sale",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
