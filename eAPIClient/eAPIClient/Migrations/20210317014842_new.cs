using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_config_data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    data = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    config_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_config_data", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_customer_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_group_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    customer_group_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
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
                name: "tbl_document_number",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    document_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    outlet_id = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    prefix = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    format = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    counter = table.Column<int>(type: "int", nullable: false),
                    counter_digit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_document_number", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_menu",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    menu_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    menu_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    text_color = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    background_color = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    root_menu_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_menu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    product_code = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_allow_discount = table.Column<bool>(type: "bit", nullable: false),
                    is_allow_free = table.Column<bool>(type: "bit", nullable: false),
                    is_inventory_product = table.Column<bool>(type: "bit", nullable: false),
                    keyword = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_price",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_portion_id = table.Column<int>(type: "int", nullable: false),
                    prices = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_price", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sale_product_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    status_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    allow_send_to_printer = table.Column<bool>(type: "bit", nullable: false),
                    allow_append_quantity = table.Column<bool>(type: "bit", nullable: false),
                    allow_display_in_pos_order_list = table.Column<bool>(type: "bit", nullable: false),
                    allow_void_or_cancel_order = table.Column<bool>(type: "bit", nullable: false),
                    active_order = table.Column<bool>(type: "bit", nullable: false),
                    submited_status_id = table.Column<int>(type: "int", nullable: false),
                    allow_send_to_printer_when_change_table = table.Column<bool>(type: "bit", nullable: false),
                    allow_send_to_printer_when_merge_table = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale_product_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sale_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    status_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    background = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    foreground = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    priority = table.Column<int>(type: "int", nullable: false),
                    is_sale_lock = table.Column<bool>(type: "bit", nullable: false),
                    is_active_order = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    shift_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_working_day",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    working_date = table.Column<DateTime>(type: "date", nullable: false),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    closed_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    closed_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    close_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    open_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    working_day_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_working_day", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    customer_group_id = table.Column<int>(type: "int", nullable: false),
                    customer_code = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    customer_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    customer_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    phone_1 = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    phone_2 = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_menu",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    menu_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_menu", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_menu_tbl_menu_menu_id",
                        column: x => x.menu_id,
                        principalTable: "tbl_menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_product_menu_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_modifier",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    modifier_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    price = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    section_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    is_multiple_select = table.Column<bool>(type: "bit", nullable: false),
                    is_section = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_modifier", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_modifier_tbl_product_modifier_parent_id",
                        column: x => x.parent_id,
                        principalTable: "tbl_product_modifier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_product_modifier_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_portion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    portion_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    cost = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    multiplier = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    unit_id = table.Column<int>(type: "int", nullable: false),
                    prices = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_portion", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_portion_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_printer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    printer_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    ip_address_port = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_printer", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_printer_tbl_product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_cashier_shift",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cashier_shift_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    working_day_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    closed_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    closed_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    open_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: true),
                    close_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: true),
                    open_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    close_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    shift = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cashier_shift", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_cashier_shift_tbl_working_day_working_day_id",
                        column: x => x.working_day_id,
                        principalTable: "tbl_working_day",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sale",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    sale_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    guest_cover = table.Column<int>(type: "int", nullable: false),
                    table_id = table.Column<int>(type: "int", nullable: false),
                    table_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    working_day_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cashier_shift_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    working_day_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    cashier_shift_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    working_date = table.Column<DateTime>(type: "date", nullable: false),
                    is_partially_paid = table.Column<bool>(type: "bit", nullable: false),
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
                    total_cost = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    taxable_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_1_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_1_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_1_taxable_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_2_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_2_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_2_taxable_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_3_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_3_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_3_taxable_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    sale_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    is_closed = table.Column<bool>(type: "bit", nullable: true),
                    closed_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    closed_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sale_tbl_cashier_shift_cashier_shift_id",
                        column: x => x.cashier_shift_id,
                        principalTable: "tbl_cashier_shift",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_sale_tbl_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "tbl_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_sale_tbl_sale_status_status_id",
                        column: x => x.status_id,
                        principalTable: "tbl_sale_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_sale_tbl_working_day_working_day_id",
                        column: x => x.working_day_id,
                        principalTable: "tbl_working_day",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    sale_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_date = table.Column<DateTime>(type: "date", nullable: false),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    payment_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    payment_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_create_payment_in_sale_order = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment", x => x.id);
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
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    sale_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    product_code = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_inventory_product = table.Column<bool>(type: "bit", nullable: false),
                    is_allow_discount = table.Column<bool>(type: "bit", nullable: false),
                    is_allow_free = table.Column<bool>(type: "bit", nullable: false),
                    portion_id = table.Column<int>(type: "int", nullable: false),
                    portion_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    kitchen_group_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    kitchen_group_sort_order = table.Column<int>(type: "int", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    multiplier = table.Column<double>(type: "float", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    status_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_free = table.Column<bool>(type: "bit", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    reqular_price = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_modifier_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    discount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    discount_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    total_discount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_revenue = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    taxable_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_1_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_1_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_1_taxable_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_2_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_2_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_2_taxable_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_3_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_3_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    tax_3_taxable_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_tax_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    deleted_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    free_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    discount_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    discount_lable = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sale_product_tbl_sale_product_status_status_id",
                        column: x => x.status_id,
                        principalTable: "tbl_sale_product_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_sale_product_tbl_sale_sale_id",
                        column: x => x.sale_id,
                        principalTable: "tbl_sale",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sale_product_modifier",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    sale_product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    modifier_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    product_modifier_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale_product_modifier", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sale_product_modifier_tbl_sale_product_sale_product_id",
                        column: x => x.sale_product_id,
                        principalTable: "tbl_sale_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cashier_shift_working_day_id",
                table: "tbl_cashier_shift",
                column: "working_day_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_customer_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_sale_id",
                table: "tbl_payment",
                column: "sale_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_menu_menu_id",
                table: "tbl_product_menu",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_menu_product_id",
                table: "tbl_product_menu",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_product_id",
                table: "tbl_product_modifier",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_portion_product_id",
                table: "tbl_product_portion",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_printer_product_id",
                table: "tbl_product_printer",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_cashier_shift_id",
                table: "tbl_sale",
                column: "cashier_shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_customer_id",
                table: "tbl_sale",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_status_id",
                table: "tbl_sale",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_working_day_id",
                table: "tbl_sale",
                column: "working_day_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_sale_id",
                table: "tbl_sale_product",
                column: "sale_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_status_id",
                table: "tbl_sale_product",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_modifier_sale_product_id",
                table: "tbl_sale_product_modifier",
                column: "sale_product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_config_data");

            migrationBuilder.DropTable(
                name: "tbl_document_number");

            migrationBuilder.DropTable(
                name: "tbl_payment");

            migrationBuilder.DropTable(
                name: "tbl_product_menu");

            migrationBuilder.DropTable(
                name: "tbl_product_modifier");

            migrationBuilder.DropTable(
                name: "tbl_product_portion");

            migrationBuilder.DropTable(
                name: "tbl_product_price");

            migrationBuilder.DropTable(
                name: "tbl_product_printer");

            migrationBuilder.DropTable(
                name: "tbl_sale_product_modifier");

            migrationBuilder.DropTable(
                name: "tbl_shift");

            migrationBuilder.DropTable(
                name: "tbl_menu");

            migrationBuilder.DropTable(
                name: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_sale_product");

            migrationBuilder.DropTable(
                name: "tbl_sale_product_status");

            migrationBuilder.DropTable(
                name: "tbl_sale");

            migrationBuilder.DropTable(
                name: "tbl_cashier_shift");

            migrationBuilder.DropTable(
                name: "tbl_customer");

            migrationBuilder.DropTable(
                name: "tbl_sale_status");

            migrationBuilder.DropTable(
                name: "tbl_working_day");

            migrationBuilder.DropTable(
                name: "tbl_customer_group");
        }
    }
}
