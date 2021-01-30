using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class bussiness_paymenttype_relation_ship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_business_branch_payment_type",
                columns: table => new
                {
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    payment_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_business_branch_payment_type", x => new { x.payment_type_id, x.business_branch_id });
                    table.ForeignKey(
                        name: "FK_tbl_business_branch_payment_type_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_tbl_business_branch_payment_type_tbl_payment_type_payment_type_id",
                        column: x => x.payment_type_id,
                        principalTable: "tbl_payment_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_payment_type_business_branch_id",
                table: "tbl_business_branch_payment_type",
                column: "business_branch_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_business_branch_payment_type");
        }
    }
}
