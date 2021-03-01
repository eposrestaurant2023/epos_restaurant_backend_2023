using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class sale_pvd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_business_branch_business_branch_id",
                table: "tbl_sale");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_customer_customer_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "grand_total_discount",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "is_new_customer",
                table: "tbl_sale");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_business_branch_business_branch_id",
                table: "tbl_sale",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_customer_customer_id",
                table: "tbl_sale",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
