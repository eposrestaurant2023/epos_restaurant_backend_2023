using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class create_stock_take_tbl_stock_take_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_vendor_purchase_order_id",
                table: "tbl_attach_files");

            migrationBuilder.AddColumn<int>(
                name: "stock_take_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "stock_take_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_stock_take",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stock_location_id = table.Column<int>(type: "int", nullable: false),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    stock_take_date = table.Column<DateTime>(type: "date", nullable: false),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    term_conditions = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    due_date = table.Column<DateTime>(type: "date", nullable: true),
                    total_quantity = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    discountable_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    stock_take_product_discount_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    discount_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    discount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    total_discount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    grand_total_discount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_stock_take", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_stock_take_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_stock_take_tbl_stock_location_stock_location_id",
                        column: x => x.stock_location_id,
                        principalTable: "tbl_stock_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_stock_take_product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    is_inventory_product = table.Column<bool>(type: "bit", nullable: false),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    multiplier = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    stock_take_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    product_type_id = table.Column<int>(type: "int", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    quantity = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    discount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    invoice_discount_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    grand_total = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    discount_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sub_total = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    total_discount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_stock_take_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_stock_take_product_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_stock_take_product_tbl_product_type_product_type_id",
                        column: x => x.product_type_id,
                        principalTable: "tbl_product_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_stock_take_product_tbl_stock_take_stock_take_id",
                        column: x => x.stock_take_id,
                        principalTable: "tbl_stock_take",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_stock_take_id",
                table: "tbl_history",
                column: "stock_take_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_stock_take_id",
                table: "tbl_attach_files",
                column: "stock_take_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_take_business_branch_id",
                table: "tbl_stock_take",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_take_stock_location_id",
                table: "tbl_stock_take",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_take_product_product_id",
                table: "tbl_stock_take_product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_take_product_product_type_id",
                table: "tbl_stock_take_product",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_take_product_stock_take_id",
                table: "tbl_stock_take_product",
                column: "stock_take_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_purchase_order_purchase_order_id",
                table: "tbl_attach_files",
                column: "purchase_order_id",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_stock_take_stock_take_id",
                table: "tbl_attach_files",
                column: "stock_take_id",
                principalTable: "tbl_stock_take",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_stock_take_stock_take_id",
                table: "tbl_history",
                column: "stock_take_id",
                principalTable: "tbl_stock_take",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_purchase_order_purchase_order_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_stock_take_stock_take_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_stock_take_stock_take_id",
                table: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_stock_take_product");

            migrationBuilder.DropTable(
                name: "tbl_stock_take");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_stock_take_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_stock_take_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "stock_take_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "stock_take_id",
                table: "tbl_attach_files");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_vendor_purchase_order_id",
                table: "tbl_attach_files",
                column: "purchase_order_id",
                principalTable: "tbl_vendor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
