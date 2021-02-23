using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class mo_group_item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_modifier_group_item",
                columns: table => new
                {
                    modifier_group_id = table.Column<int>(type: "int", nullable: false),
                    modifier_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_modifier_group_item", x => new { x.modifier_group_id, x.modifier_id });
                    table.ForeignKey(
                        name: "FK_tbl_modifier_group_item_tbl_modifier_group_modifier_group_id",
                        column: x => x.modifier_group_id,
                        principalTable: "tbl_modifier_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_modifier_group_item_tbl_modifier_modifier_id",
                        column: x => x.modifier_id,
                        principalTable: "tbl_modifier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_item_modifier_id",
                table: "tbl_modifier_group_item",
                column: "modifier_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_modifier_group_item");
        }
    }
}
