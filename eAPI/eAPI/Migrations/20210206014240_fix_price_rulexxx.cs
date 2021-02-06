using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_price_rulexxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_business_branch_product_price",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    portion_id = table.Column<int>(type: "int", nullable: false),
                    price_rule_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    is_default = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_business_branch_product_price", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_business_branch_product_price_tbl_product_portion_portion_id",
                        column: x => x.portion_id,
                        principalTable: "tbl_product_portion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_product_price_portion_id",
                table: "tbl_business_branch_product_price",
                column: "portion_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_business_branch_product_price");
        }
    }
}
