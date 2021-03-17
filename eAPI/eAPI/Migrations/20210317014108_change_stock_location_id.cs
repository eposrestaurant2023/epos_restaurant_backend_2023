using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_stock_location_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_stock_location_stock_location_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_tbl_stock_location_stock_location_id",
                table: "tbl_purchase_order");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_take_tbl_stock_location_stock_location_id",
                table: "tbl_stock_take");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_tbl_stock_location_from_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_tbl_stock_location_to_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropTable(
                name: "tbl_stock_location_product");

            migrationBuilder.DropTable(
                name: "tbl_stock_location");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_from_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_to_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_take_stock_location_id",
                table: "tbl_stock_take");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_stock_location_id",
                table: "tbl_purchase_order");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_transaction_stock_location_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "from_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "to_stock_location_id",
                table: "tbl_stock_transfer");

            migrationBuilder.DropColumn(
                name: "stock_location_id",
                table: "tbl_stock_take");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "from_stock_location_id",
                table: "tbl_stock_transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "to_stock_location_id",
                table: "tbl_stock_transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "stock_location_id",
                table: "tbl_stock_take",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbl_stock_location",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_default = table.Column<bool>(type: "bit", nullable: false),
                    stock_location_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_stock_location", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_stock_location_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_stock_location_product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    initial_adjustment_quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    initial_quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    max_quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    min_quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    multiplier = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    stock_location_id = table.Column<int>(type: "int", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
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

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_from_stock_location_id",
                table: "tbl_stock_transfer",
                column: "from_stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_to_stock_location_id",
                table: "tbl_stock_transfer",
                column: "to_stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_take_stock_location_id",
                table: "tbl_stock_take",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_stock_location_id",
                table: "tbl_purchase_order",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_transaction_stock_location_id",
                table: "tbl_inventory_transaction",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_business_branch_id",
                table: "tbl_stock_location",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_product_product_id",
                table: "tbl_stock_location_product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_product_stock_location_id",
                table: "tbl_stock_location_product",
                column: "stock_location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_transaction_tbl_stock_location_stock_location_id",
                table: "tbl_inventory_transaction",
                column: "stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_purchase_order_tbl_stock_location_stock_location_id",
                table: "tbl_purchase_order",
                column: "stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_take_tbl_stock_location_stock_location_id",
                table: "tbl_stock_take",
                column: "stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_tbl_stock_location_from_stock_location_id",
                table: "tbl_stock_transfer",
                column: "from_stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_tbl_stock_location_to_stock_location_id",
                table: "tbl_stock_transfer",
                column: "to_stock_location_id",
                principalTable: "tbl_stock_location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
