using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_discountxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_system_feature_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_system_feature");

            migrationBuilder.DropIndex(
                name: "IX_tbl_system_feature_BusinessBranchModelid",
                table: "tbl_system_feature");

            migrationBuilder.DropColumn(
                name: "BusinessBranchModelid",
                table: "tbl_system_feature");

            migrationBuilder.RenameColumn(
                name: "discount_type",
                table: "tbl_sale_product",
                newName: "sale_product_discount_type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sale_product_discount_type",
                table: "tbl_sale_product",
                newName: "discount_type");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessBranchModelid",
                table: "tbl_system_feature",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_system_feature_BusinessBranchModelid",
                table: "tbl_system_feature",
                column: "BusinessBranchModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_system_feature_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_system_feature",
                column: "BusinessBranchModelid",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
