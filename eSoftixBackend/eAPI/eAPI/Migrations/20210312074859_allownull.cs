using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class allownull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_contact_tbl_customer_customer_id",
                table: "tbl_contact");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_contact_tbl_project_project_id",
                table: "tbl_contact");

            migrationBuilder.AlterColumn<int>(
                name: "project_id",
                table: "tbl_contact",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "customer_id",
                table: "tbl_contact",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_contact_tbl_customer_customer_id",
                table: "tbl_contact",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_contact_tbl_project_project_id",
                table: "tbl_contact",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_contact_tbl_customer_customer_id",
                table: "tbl_contact");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_contact_tbl_project_project_id",
                table: "tbl_contact");

            migrationBuilder.AlterColumn<int>(
                name: "project_id",
                table: "tbl_contact",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "customer_id",
                table: "tbl_contact",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_contact_tbl_customer_customer_id",
                table: "tbl_contact",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_contact_tbl_project_project_id",
                table: "tbl_contact",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
