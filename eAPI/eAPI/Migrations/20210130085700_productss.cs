using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class productss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_menu",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    menu_name_en = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "Khmer_100_BIN"),
                    menu_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_menu", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_menu_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_menu_tbl_menu_parent_id",
                        column: x => x.parent_id,
                        principalTable: "tbl_menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_modifier",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modifier_name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_modifier", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_price_rule",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_price_rule", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_modifier",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modifier_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_product_modifier_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_business_branch_price_rule",
                columns: table => new
                {
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    price_rule_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_business_branch_price_rule", x => new { x.business_branch_id, x.price_rule_id });
                    table.ForeignKey(
                        name: "FK_tbl_business_branch_price_rule_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_business_branch_price_rule_tbl_price_rule_price_rule_id",
                        column: x => x.price_rule_id,
                        principalTable: "tbl_price_rule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_price",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    price_rule_id = table.Column<int>(type: "int", nullable: false),
                    portion_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "Khmer_100_BIN"),
                    multiplier = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    is_default = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_price", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_price_tbl_price_rule_price_rule_id",
                        column: x => x.price_rule_id,
                        principalTable: "tbl_price_rule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_product_price_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_price_rule_price_rule_id",
                table: "tbl_business_branch_price_rule",
                column: "price_rule_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_menu_business_branch_id",
                table: "tbl_menu",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_menu_parent_id",
                table: "tbl_menu",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_modifier_id",
                table: "tbl_product_modifier",
                column: "modifier_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_product_id",
                table: "tbl_product_modifier",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_price_price_rule_id",
                table: "tbl_product_price",
                column: "price_rule_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_price_product_id",
                table: "tbl_product_price",
                column: "product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_business_branch_price_rule");

            migrationBuilder.DropTable(
                name: "tbl_menu");

            migrationBuilder.DropTable(
                name: "tbl_product_modifier");

            migrationBuilder.DropTable(
                name: "tbl_product_price");

            migrationBuilder.DropTable(
                name: "tbl_modifier");

            migrationBuilder.DropTable(
                name: "tbl_price_rule");
        }
    }
}
