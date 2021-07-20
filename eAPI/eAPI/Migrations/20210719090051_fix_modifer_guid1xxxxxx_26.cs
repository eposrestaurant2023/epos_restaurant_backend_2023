using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_modifer_guid1xxxxxx_26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_product_modifier",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    modifier_group_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    modifier_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    section_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    is_multiple_select = table.Column<bool>(type: "bit", nullable: false),
                    is_section = table.Column<bool>(type: "bit", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_modifier", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                        column: x => x.modifier_id,
                        principalTable: "tbl_modifier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_tbl_product_modifier_modifier_id",
                table: "tbl_product_modifier",
                column: "modifier_id");

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
