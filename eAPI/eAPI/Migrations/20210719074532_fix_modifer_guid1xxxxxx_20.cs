using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_modifer_guid1xxxxxx_20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_modifier_group_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modifier_group_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    is_section = table.Column<bool>(type: "bit", nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    is_multiple_select = table.Column<bool>(type: "bit", nullable: false),
                    section_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    price = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_modifier_group_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_modifier_group_item_tbl_modifier_group_item_parent_id",
                        column: x => x.parent_id,
                        principalTable: "tbl_modifier_group_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_modifier_group_item_tbl_modifier_group_modifier_group_id",
                        column: x => x.modifier_group_id,
                        principalTable: "tbl_modifier_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_modifier_group_item_tbl_modifier_modifier_id",
                        column: x => x.modifier_id,
                        principalTable: "tbl_modifier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_item_modifier_group_id",
                table: "tbl_modifier_group_item",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_item_modifier_id",
                table: "tbl_modifier_group_item",
                column: "modifier_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_item_parent_id",
                table: "tbl_modifier_group_item",
                column: "parent_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_modifier_group_item");
        }
    }
}
