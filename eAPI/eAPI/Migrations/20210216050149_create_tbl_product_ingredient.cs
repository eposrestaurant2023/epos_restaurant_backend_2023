using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class create_tbl_product_ingredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_product_ingredient",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_menu_id = table.Column<int>(type: "int", nullable: false),
                    product_ingredient_id = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_ingredient", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_ingredient_tbl_product_product_ingredient_id",
                        column: x => x.product_ingredient_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_product_ingredient_tbl_product_product_menu_id",
                        column: x => x.product_menu_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_ingredient_product_ingredient_id",
                table: "tbl_product_ingredient",
                column: "product_ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_ingredient_product_menu_id",
                table: "tbl_product_ingredient",
                column: "product_menu_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_product_ingredient");
        }
    }
}
