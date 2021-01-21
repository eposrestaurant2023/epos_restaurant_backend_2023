using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class test_guid_insertx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_sub");

            migrationBuilder.DropTable(
                name: "tbl_main");

            migrationBuilder.AddColumn<int>(
                name: "ProductModelid",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_product_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_category = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    product_category_kh = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    background_color = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    prefix = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    digit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    format = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    counter = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    product_type_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    background_color = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantity = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    total_variants = table.Column<int>(type: "int", nullable: false),
                    keyword = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_outlet_id = table.Column<int>(type: "int", nullable: false),
                    is_auto_generate_product_code = table.Column<bool>(type: "bit", nullable: false),
                    product_type_id = table.Column<int>(type: "int", nullable: false),
                    product_category_id = table.Column<int>(type: "int", nullable: false),
                    product_name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    product_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    product_code = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_code_1 = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_code_2 = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_image = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    price = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_allow_discount = table.Column<bool>(type: "bit", nullable: false),
                    background_color = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    text_color = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    has_variant = table.Column<bool>(type: "bit", nullable: false),
                    track_quantity_on_variant = table.Column<bool>(type: "bit", nullable: false),
                    use_variant_1 = table.Column<bool>(type: "bit", nullable: false),
                    variant_1_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    use_variant_2 = table.Column<bool>(type: "bit", nullable: false),
                    variant_2_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    use_variant_3 = table.Column<bool>(type: "bit", nullable: false),
                    variant_3_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    use_variant_price = table.Column<bool>(type: "bit", nullable: false),
                    is_inventory_product = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_tbl_product_category_product_category_id",
                        column: x => x.product_category_id,
                        principalTable: "tbl_product_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_product_tbl_product_type_product_type_id",
                        column: x => x.product_type_id,
                        principalTable: "tbl_product_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_ProductModelid",
                table: "tbl_history",
                column: "ProductModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_product_category_id",
                table: "tbl_product",
                column: "product_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_product_type_id",
                table: "tbl_product",
                column: "product_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_product_ProductModelid",
                table: "tbl_history",
                column: "ProductModelid",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_product_ProductModelid",
                table: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_product_category");

            migrationBuilder.DropTable(
                name: "tbl_product_type");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_ProductModelid",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "ProductModelid",
                table: "tbl_history");

            migrationBuilder.CreateTable(
                name: "tbl_main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    main_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_main", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sub",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    main_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sub_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sub", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sub_tbl_main_main_id",
                        column: x => x.main_id,
                        principalTable: "tbl_main",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sub_main_id",
                table: "tbl_sub",
                column: "main_id");
        }
    }
}
