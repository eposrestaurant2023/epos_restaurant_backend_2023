using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class is_deleted_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_tbl_customer_customer_id",
                table: "Contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "tbl_contact");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_customer_id",
                table: "tbl_contact",
                newName: "IX_tbl_contact_customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_contact",
                table: "tbl_contact",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_contact_tbl_customer_customer_id",
                table: "tbl_contact",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_contact_tbl_customer_customer_id",
                table: "tbl_contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_contact",
                table: "tbl_contact");

            migrationBuilder.RenameTable(
                name: "tbl_contact",
                newName: "Contacts");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_contact_customer_id",
                table: "Contacts",
                newName: "IX_Contacts_customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_tbl_customer_customer_id",
                table: "Contacts",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
