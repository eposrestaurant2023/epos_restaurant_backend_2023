using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_invtory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_inventory_transaction_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inventory_transaction_type_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_inventory_transaction_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_stock_location_product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stock_location_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    min_quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    max_quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_stock_location_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_stock_location_product_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_stock_location_product_tbl_stock_location_stock_location_id",
                        column: x => x.stock_location_id,
                        principalTable: "tbl_stock_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_inventory_transaction",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transaction_date = table.Column<DateTime>(type: "date", nullable: false),
                    inventory_transaction_type_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    stock_location_id = table.Column<int>(type: "int", nullable: false),
                    old_quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    quantity_on_hand = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    sale_id = table.Column<int>(type: "int", nullable: true),
                    purchase_order_id = table.Column<int>(type: "int", nullable: true),
                    stock_transfer_id = table.Column<int>(type: "int", nullable: true),
                    stock_take_id = table.Column<int>(type: "int", nullable: true),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_inventory_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_inventory_transaction_tbl_inventory_transaction_type_inventory_transaction_type_id",
                        column: x => x.inventory_transaction_type_id,
                        principalTable: "tbl_inventory_transaction_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_inventory_transaction_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_inventory_transaction_tbl_stock_location_stock_location_id",
                        column: x => x.stock_location_id,
                        principalTable: "tbl_stock_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_inventory_transaction_type_id",
                table: "tbl_inventory_transaction",
                column: "inventory_transaction_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_product_id",
                table: "tbl_inventory_transaction",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_stock_location_id",
                table: "tbl_inventory_transaction",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_product_product_id",
                table: "tbl_stock_location_product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_product_stock_location_id",
                table: "tbl_stock_location_product",
                column: "stock_location_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_inventory_transaction");

            migrationBuilder.DropTable(
                name: "tbl_stock_location_product");

            migrationBuilder.DropTable(
                name: "tbl_inventory_transaction_type");
        }
    }
}
