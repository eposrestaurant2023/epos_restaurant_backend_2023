using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_history_tablexx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderPaymentModelid = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    last_modified_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modifier_group_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modifier_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    module = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    old_amount = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    production_id = table.Column<int>(type: "int", nullable: true),
                    purchase_order_id = table.Column<int>(type: "int", nullable: true),
                    sale_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    sale_payment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    stock_take_id = table.Column<int>(type: "int", nullable: true),
                    stock_transfer_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    transaction_date = table.Column<DateTime>(type: "date", nullable: true),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    vendor_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "tbl_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_modifier_group_modifier_group_id",
                        column: x => x.modifier_group_id,
                        principalTable: "tbl_modifier_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_modifier_modifier_id",
                        column: x => x.modifier_id,
                        principalTable: "tbl_modifier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_production_production_id",
                        column: x => x.production_id,
                        principalTable: "tbl_production",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_purchase_order_payment_PurchaseOrderPaymentModelid",
                        column: x => x.PurchaseOrderPaymentModelid,
                        principalTable: "tbl_purchase_order_payment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_purchase_order_purchase_order_id",
                        column: x => x.purchase_order_id,
                        principalTable: "tbl_purchase_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_sale_payment_sale_payment_id",
                        column: x => x.sale_payment_id,
                        principalTable: "tbl_sale_payment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_sale_sale_id",
                        column: x => x.sale_id,
                        principalTable: "tbl_sale",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_stock_take_stock_take_id",
                        column: x => x.stock_take_id,
                        principalTable: "tbl_stock_take",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_stock_transfer_stock_transfer_id",
                        column: x => x.stock_transfer_id,
                        principalTable: "tbl_stock_transfer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_vendor_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "tbl_vendor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_customer_id",
                table: "tbl_history",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_modifier_group_id",
                table: "tbl_history",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_modifier_id",
                table: "tbl_history",
                column: "modifier_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_product_id",
                table: "tbl_history",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_production_id",
                table: "tbl_history",
                column: "production_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_purchase_order_id",
                table: "tbl_history",
                column: "purchase_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_PurchaseOrderPaymentModelid",
                table: "tbl_history",
                column: "PurchaseOrderPaymentModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_sale_id",
                table: "tbl_history",
                column: "sale_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_sale_payment_id",
                table: "tbl_history",
                column: "sale_payment_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_stock_take_id",
                table: "tbl_history",
                column: "stock_take_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_stock_transfer_id",
                table: "tbl_history",
                column: "stock_transfer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_user_id",
                table: "tbl_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_vendor_id",
                table: "tbl_history",
                column: "vendor_id");
        }
    }
}
