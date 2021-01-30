using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class remove_bussiness_branch_forein_key_from_payment_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_type_tbl_business_branch_business_branch_id",
                table: "tbl_payment_type");

            migrationBuilder.DropIndex(
                name: "IX_tbl_payment_type_business_branch_id",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_payment_type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_payment_type",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_type_business_branch_id",
                table: "tbl_payment_type",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_type_tbl_business_branch_business_branch_id",
                table: "tbl_payment_type",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
