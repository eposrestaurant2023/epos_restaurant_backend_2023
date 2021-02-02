using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class create_tbl_sale_tbl_sale_product_09 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentModelid",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SaleModelid",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_sale",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
                    stock_location_id = table.Column<int>(type: "int", nullable: false),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sale_date = table.Column<DateTime>(type: "date", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    term_conditions = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sale_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
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
                    is_new_customer = table.Column<bool>(type: "bit", nullable: false),
                    total_cost = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
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
                    table.PrimaryKey("PK_tbl_sale", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sale_tbl_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "tbl_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_sale_tbl_outlet_outlet_id",
                        column: x => x.outlet_id,
                        principalTable: "tbl_outlet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_sale_tbl_stock_location_stock_location_id",
                        column: x => x.stock_location_id,
                        principalTable: "tbl_stock_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    payment_date = table.Column<DateTime>(type: "date", nullable: false),
                    sale_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
                    payment_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    payment_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_create_payment_in_sale_order = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_payment_tbl_payment_type_payment_type_id",
                        column: x => x.payment_type_id,
                        principalTable: "tbl_payment_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_payment_tbl_sale_sale_id",
                        column: x => x.sale_id,
                        principalTable: "tbl_sale",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sale_product",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_inventory_product = table.Column<bool>(type: "bit", nullable: false),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    multiplier = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    sale_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    product_type_id = table.Column<int>(type: "int", nullable: false),
                    is_allow_discount = table.Column<bool>(type: "bit", nullable: false),
                    sale_product_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
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
                    table.PrimaryKey("PK_tbl_sale_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sale_product_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_sale_product_tbl_product_type_product_type_id",
                        column: x => x.product_type_id,
                        principalTable: "tbl_product_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_sale_product_tbl_sale_sale_id",
                        column: x => x.sale_id,
                        principalTable: "tbl_sale",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_PaymentModelid",
                table: "tbl_history",
                column: "PaymentModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_SaleModelid",
                table: "tbl_history",
                column: "SaleModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_payment_type_id",
                table: "tbl_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_sale_id",
                table: "tbl_payment",
                column: "sale_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_customer_id",
                table: "tbl_sale",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_outlet_id",
                table: "tbl_sale",
                column: "outlet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_stock_location_id",
                table: "tbl_sale",
                column: "stock_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_product_id",
                table: "tbl_sale_product",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_product_type_id",
                table: "tbl_sale_product",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_sale_id",
                table: "tbl_sale_product",
                column: "sale_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_payment_PaymentModelid",
                table: "tbl_history",
                column: "PaymentModelid",
                principalTable: "tbl_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_sale_SaleModelid",
                table: "tbl_history",
                column: "SaleModelid",
                principalTable: "tbl_sale",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_payment_PaymentModelid",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_sale_SaleModelid",
                table: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_payment");


            migrationBuilder.DropTable(
                name: "tbl_sale_product");

            migrationBuilder.DropTable(
                name: "tbl_sale");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_PaymentModelid",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_SaleModelid",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "PaymentModelid",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "SaleModelid",
                table: "tbl_history");

            migrationBuilder.AddColumn<int>(
                name: "product_type_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_product_type_product_type_id",
                table: "tbl_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
