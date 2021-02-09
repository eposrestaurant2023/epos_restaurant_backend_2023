using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class tbl_create_PO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderModelid",
                table: "tbl_payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderModelid",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderPaymentModelid",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_purchase_order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stock_location_id = table.Column<int>(type: "int", nullable: false),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    purchase_date = table.Column<DateTime>(type: "date", nullable: false),
                    vendor_id = table.Column<int>(type: "int", nullable: false),
                    discount_user_id = table.Column<int>(type: "int", nullable: true),
                    vendor_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    term_conditions = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    purchase_order_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_partially_paid = table.Column<bool>(type: "bit", nullable: false),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    is_over_due = table.Column<bool>(type: "bit", nullable: false),
                    due_date = table.Column<DateTime>(type: "date", nullable: true),
                    total_quantity = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    discountable_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    sale_product_discount_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    discount_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    discount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    total_discount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    paid_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    is_paid = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_tbl_purchase_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_tbl_stock_location_stock_location_id",
                        column: x => x.stock_location_id,
                        principalTable: "tbl_stock_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_tbl_user_discount_user_id",
                        column: x => x.discount_user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_tbl_vendor_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "tbl_vendor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_purchase_order_payment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    payment_date = table.Column<DateTime>(type: "date", nullable: false),
                    purhcase_order_id = table.Column<int>(type: "int", nullable: true),
                    payment_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    payment_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_create_payment_in_puchase_order = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_purchase_order_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_payment_tbl_payment_type_payment_type_id",
                        column: x => x.payment_type_id,
                        principalTable: "tbl_payment_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_payment_tbl_purchase_order_purhcase_order_id",
                        column: x => x.purhcase_order_id,
                        principalTable: "tbl_purchase_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_purchase_order_product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    is_inventory_product = table.Column<bool>(type: "bit", nullable: false),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    multiplier = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    purchase_order_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    product_type_id = table.Column<int>(type: "int", nullable: false),
                    is_allow_discount = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    quantity = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    regular_price = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    selling_price = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
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
                    table.PrimaryKey("PK_tbl_purchase_order_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_product_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_product_tbl_product_type_product_type_id",
                        column: x => x.product_type_id,
                        principalTable: "tbl_product_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_product_tbl_purchase_order_purchase_order_id",
                        column: x => x.purchase_order_id,
                        principalTable: "tbl_purchase_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_PurchaseOrderModelid",
                table: "tbl_payment",
                column: "PurchaseOrderModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_PurchaseOrderModelid",
                table: "tbl_history",
                column: "PurchaseOrderModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_PurchaseOrderPaymentModelid",
                table: "tbl_history",
                column: "PurchaseOrderPaymentModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_business_branch_id",
                table: "tbl_purchase_order",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_discount_user_id",
                table: "tbl_purchase_order",
                column: "discount_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_stock_location_id",
                table: "tbl_purchase_order",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_vendor_id",
                table: "tbl_purchase_order",
                column: "vendor_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_payment_payment_type_id",
                table: "tbl_purchase_order_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_payment_purhcase_order_id",
                table: "tbl_purchase_order_payment",
                column: "purhcase_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_product_product_id",
                table: "tbl_purchase_order_product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_product_product_type_id",
                table: "tbl_purchase_order_product",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_product_purchase_order_id",
                table: "tbl_purchase_order_product",
                column: "purchase_order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_PurchaseOrderPaymentModelid",
                table: "tbl_history",
                column: "PurchaseOrderPaymentModelid",
                principalTable: "tbl_purchase_order_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_PurchaseOrderModelid",
                table: "tbl_history",
                column: "PurchaseOrderModelid",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_tbl_purchase_order_PurchaseOrderModelid",
                table: "tbl_payment",
                column: "PurchaseOrderModelid",
                principalTable: "tbl_purchase_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_payment_PurchaseOrderPaymentModelid",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_purchase_order_PurchaseOrderModelid",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_tbl_purchase_order_PurchaseOrderModelid",
                table: "tbl_payment");

            migrationBuilder.DropTable(
                name: "tbl_purchase_order_payment");

            migrationBuilder.DropTable(
                name: "tbl_purchase_order_product");

            migrationBuilder.DropTable(
                name: "tbl_purchase_order");

            migrationBuilder.DropIndex(
                name: "IX_tbl_payment_PurchaseOrderModelid",
                table: "tbl_payment");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_PurchaseOrderModelid",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_PurchaseOrderPaymentModelid",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderModelid",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderModelid",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderPaymentModelid",
                table: "tbl_history");
        }
    }
}
