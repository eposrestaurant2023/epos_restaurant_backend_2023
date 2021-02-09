using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class aa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutletModel",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    outlet_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    outlet_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutletModel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_currency",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_format = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    change_exchange_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_customer_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_group_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    customer_group_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_customer_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    product_type_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payment_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    payment_type_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    payment_type_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_build_in = table.Column<bool>(type: "bit", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_payment_type_tbl_currency_currency_id",
                        column: x => x.currency_id,
                        principalTable: "tbl_currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_group_id = table.Column<int>(type: "int", nullable: false),
                    business_branch_id = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    customer_code = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    customer_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    customer_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, collation: "Khmer_100_BIN"),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    phone_1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    phone_2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_customer", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                        column: x => x.customer_group_id,
                        principalTable: "tbl_customer_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sale",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sale_date = table.Column<DateTime>(type: "date", nullable: false),
                    customer_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    term_conditions = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sale_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_partially_paid = table.Column<bool>(type: "bit", nullable: false),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    is_over_due = table.Column<bool>(type: "bit", nullable: false),
                    due_date = table.Column<DateTime>(type: "date", nullable: true),
                    total_quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    discountable_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    sale_product_discount_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    discount_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    discount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_discount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    paid_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    is_paid = table.Column<bool>(type: "bit", nullable: false),
                    is_new_customer = table.Column<bool>(type: "bit", nullable: false),
                    total_cost = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    grand_total_discount = table.Column<decimal>(type: "decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sale_OutletModel_outlet_id",
                        column: x => x.outlet_id,
                        principalTable: "OutletModel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_sale_tbl_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "tbl_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleProductShareModel",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_inventory_product = table.Column<bool>(type: "bit", nullable: false),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    multiplier = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    is_allow_discount = table.Column<bool>(type: "bit", nullable: false),
                    sale_product_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    regular_price = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    selling_price = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    discount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    invoice_discount_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    grand_total = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    discount_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sub_total = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_discount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    SaleModelid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleProductShareModel", x => x.id);
                    table.ForeignKey(
                        name: "FK_SaleProductShareModel_tbl_sale_SaleModelid",
                        column: x => x.SaleModelid,
                        principalTable: "tbl_sale",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    sale_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_date = table.Column<DateTime>(type: "date", nullable: false),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
                    payment_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    payment_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_create_payment_in_sale_order = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_payment_tbl_payment_type_payment_type_id",
                        column: x => x.payment_type_id,
                        principalTable: "tbl_payment_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                    sale_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    product_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sale_product_SaleProductShareModel_id",
                        column: x => x.id,
                        principalTable: "SaleProductShareModel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_SaleProductShareModel_SaleModelid",
                table: "SaleProductShareModel",
                column: "SaleModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_customer_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_payment_type_id",
                table: "tbl_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_sale_id",
                table: "tbl_payment",
                column: "sale_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_type_currency_id",
                table: "tbl_payment_type",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_customer_id",
                table: "tbl_sale",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_outlet_id",
                table: "tbl_sale",
                column: "outlet_id");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_payment");

            migrationBuilder.DropTable(
                name: "tbl_sale_product");

            migrationBuilder.DropTable(
                name: "tbl_payment_type");

            migrationBuilder.DropTable(
                name: "SaleProductShareModel");

            migrationBuilder.DropTable(
                name: "tbl_product_type");

            migrationBuilder.DropTable(
                name: "tbl_currency");

            migrationBuilder.DropTable(
                name: "tbl_sale");

            migrationBuilder.DropTable(
                name: "OutletModel");

            migrationBuilder.DropTable(
                name: "tbl_customer");

            migrationBuilder.DropTable(
                name: "tbl_customer_group");
        }
    }
}
