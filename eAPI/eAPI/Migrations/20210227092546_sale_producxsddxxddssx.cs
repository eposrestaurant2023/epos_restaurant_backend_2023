using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class sale_producxsddxxddssx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_tbl_sale_product_status_status_id",
                table: "tbl_sale_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_product_status_id",
                table: "tbl_sale_product");

            migrationBuilder.CreateTable(
                name: "tbl_sale_product_modifier",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    sale_product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    modifier_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_modifier_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale_product_modifier", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sale_product_modifier_tbl_sale_product_sale_product_id",
                        column: x => x.sale_product_id,
                        principalTable: "tbl_sale_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_modifier_sale_product_id",
                table: "tbl_sale_product_modifier",
                column: "sale_product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_sale_product_modifier");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_status_id",
                table: "tbl_sale_product",
                column: "status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_tbl_sale_product_status_status_id",
                table: "tbl_sale_product",
                column: "status_id",
                principalTable: "tbl_sale_product_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
