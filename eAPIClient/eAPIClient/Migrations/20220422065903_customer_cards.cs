using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class customer_cards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCards_tbl_customer_customer_id",
                table: "CustomerCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerCards",
                table: "CustomerCards");

            migrationBuilder.RenameTable(
                name: "CustomerCards",
                newName: "tbl_customer_card");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCards_customer_id",
                table: "tbl_customer_card",
                newName: "IX_tbl_customer_card_customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_customer_card",
                table: "tbl_customer_card",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_customer_card_tbl_customer_customer_id",
                table: "tbl_customer_card",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_customer_card_tbl_customer_customer_id",
                table: "tbl_customer_card");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_customer_card",
                table: "tbl_customer_card");

            migrationBuilder.RenameTable(
                name: "tbl_customer_card",
                newName: "CustomerCards");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_customer_card_customer_id",
                table: "CustomerCards",
                newName: "IX_CustomerCards_customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerCards",
                table: "CustomerCards",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCards_tbl_customer_customer_id",
                table: "CustomerCards",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
