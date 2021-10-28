using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class create_production_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "production_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_production",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stock_location_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    stock_take_date = table.Column<DateTime>(type: "date", nullable: false),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    term_conditions = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    total_quantity = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    last_modified_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_production", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_production_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_production_tbl_stock_location_stock_location_id",
                        column: x => x.stock_location_id,
                        principalTable: "tbl_stock_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_production_product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    is_inventory_product = table.Column<bool>(type: "bit", nullable: false),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    production_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    quantity = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    regular_cost = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    grand_total = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    multiplier = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    last_modified_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_production_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_production_product_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_production_product_tbl_production_production_id",
                        column: x => x.production_id,
                        principalTable: "tbl_production",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_production_id",
                table: "tbl_history",
                column: "production_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_production_business_branch_id",
                table: "tbl_production",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_production_stock_location_id",
                table: "tbl_production",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_production_product_product_id",
                table: "tbl_production_product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_production_product_production_id",
                table: "tbl_production_product",
                column: "production_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_production_production_id",
                table: "tbl_history",
                column: "production_id",
                principalTable: "tbl_production",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_production_production_id",
                table: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_production_product");

            migrationBuilder.DropTable(
                name: "tbl_production");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_production_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "production_id",
                table: "tbl_history");
        }
    }
}
