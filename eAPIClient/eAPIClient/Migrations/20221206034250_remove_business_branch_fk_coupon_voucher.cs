using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class remove_business_branch_fk_coupon_voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_coupon_voucher_transaction_BusinessBranchModel_business_branch_id",
                table: "tbl_coupon_voucher_transaction");

            migrationBuilder.DropTable(
                name: "BusinessBranchModel");

            migrationBuilder.DropIndex(
                name: "IX_tbl_coupon_voucher_transaction_business_branch_id",
                table: "tbl_coupon_voucher_transaction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessBranchModel",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    address_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    address_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    business_branch_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    business_branch_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    contact_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    phone_1 = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    phone_2 = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    website = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessBranchModel", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_coupon_voucher_transaction_business_branch_id",
                table: "tbl_coupon_voucher_transaction",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_coupon_voucher_transaction_BusinessBranchModel_business_branch_id",
                table: "tbl_coupon_voucher_transaction",
                column: "business_branch_id",
                principalTable: "BusinessBranchModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
