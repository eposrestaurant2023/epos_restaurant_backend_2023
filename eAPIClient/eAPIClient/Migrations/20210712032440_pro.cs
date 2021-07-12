using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class pro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_product_modifier",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    modifier_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    price = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    section_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    is_multiple_select = table.Column<bool>(type: "bit", nullable: false),
                    is_section = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_modifier", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_modifier_tbl_product_modifier_parent_id",
                        column: x => x.parent_id,
                        principalTable: "tbl_product_modifier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_product_modifier_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_product_id",
                table: "tbl_product_modifier",
                column: "product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_product_modifier");
        }
    }
}
