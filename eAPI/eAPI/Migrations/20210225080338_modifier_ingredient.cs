using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class modifier_ingredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_modifier_ingredient",
                columns: table => new
                {
                    modifier_id = table.Column<int>(type: "int", nullable: false),
                    ingredient_id = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_modifier_ingredient", x => new { x.modifier_id, x.ingredient_id });
                    table.ForeignKey(
                        name: "FK_tbl_modifier_ingredient_tbl_modifier_modifier_id",
                        column: x => x.modifier_id,
                        principalTable: "tbl_modifier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_modifier_ingredient_tbl_product_ingredient_id",
                        column: x => x.ingredient_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_ingredient_ingredient_id",
                table: "tbl_modifier_ingredient",
                column: "ingredient_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_modifier_ingredient");
        }
    }
}
