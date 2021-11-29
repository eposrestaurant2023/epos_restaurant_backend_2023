using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_inventory_checked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_test");

            migrationBuilder.AddColumn<Guid>(
                name: "inventory_check_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_inventory_check",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stock_location_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    inventory_check_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    total_cost = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_categories = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
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
                    table.PrimaryKey("PK_tbl_inventory_check", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_inventory_check_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_inventory_check_tbl_stock_location_stock_location_id",
                        column: x => x.stock_location_id,
                        principalTable: "tbl_stock_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "tbl_inventory_check_product",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    cost = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    initial_quantity = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    receive_quantity = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    consume_quantity = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    expected_quantity = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    actual_quantity = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    diference_quantity = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    diference_amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    InventoryCheckModelid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_tbl_inventory_check_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_inventory_check_product_tbl_inventory_check_InventoryCheckModelid",
                        column: x => x.InventoryCheckModelid,
                        principalTable: "tbl_inventory_check",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_inventory_check_id",
                table: "tbl_history",
                column: "inventory_check_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_check_business_branch_id",
                table: "tbl_inventory_check",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_check_stock_location_id",
                table: "tbl_inventory_check",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_check_product_InventoryCheckModelid",
                table: "tbl_inventory_check_product",
                column: "InventoryCheckModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_inventory_check_inventory_check_id",
                table: "tbl_history",
                column: "inventory_check_id",
                principalTable: "tbl_inventory_check",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_inventory_check_inventory_check_id",
                table: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_inventory_check_product");

            migrationBuilder.DropTable(
                name: "tbl_inventory_check");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_inventory_check_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "inventory_check_id",
                table: "tbl_history");

            migrationBuilder.CreateTable(
                name: "tbl_test",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_test", x => x.id);
                });
        }
    }
}
