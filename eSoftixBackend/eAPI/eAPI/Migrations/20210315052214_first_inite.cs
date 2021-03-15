using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class first_inite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreProcedureResultModel",
                columns: table => new
                {
                    result = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tbl_business_branch",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_branch_name_en = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "Khmer_100_BIN"),
                    business_branch_name_kh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    address_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    address_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    phone_1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    phone_2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    website = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    other = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_business_branch", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_category_note",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    category_note_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    category_note_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    is_multiple_select = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_category_note", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_country",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    country_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_country", x => x.id);
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
                name: "tbl_document_number",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    document_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
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
                name: "tbl_module_view",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    module_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    default_filters = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    default_order_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    default_order_by_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    permission_option = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_default = table.Column<bool>(type: "bit", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_module_view", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_number",
                columns: table => new
                {
                    number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tbl_permission_option",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    option_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    roles = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    show_in_menu = table.Column<bool>(type: "bit", nullable: false),
                    icon = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    url = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    is_match_all = table.Column<bool>(type: "bit", nullable: false),
                    report_title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    report_title_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    is_front_end = table.Column<bool>(type: "bit", nullable: false),
                    is_checked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_permission_option", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_permission_option_tbl_permission_option_parent_id",
                        column: x => x.parent_id,
                        principalTable: "tbl_permission_option",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_project_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_type_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
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
                    table.PrimaryKey("PK_tbl_project_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_request_license",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
                    station_id = table.Column<int>(type: "int", nullable: false),
                    expired_date = table.Column<DateTime>(type: "date", nullable: false),
                    is_full_license = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_request_license", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_buildin = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    setting_value = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    setting_title = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    setting_description = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    input_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_setting", x => x.id);
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
                    exchange_rate = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    change_exchange_rate = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_currency", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_currency_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_outlets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    outlet_name_en = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "Khmer_100_BIN"),
                    outlet_name_kh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_outlets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_outlets_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category_note_id = table.Column<int>(type: "int", nullable: false),
                    note_label = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notes_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_tbl_category_note_category_note_id",
                        column: x => x.category_note_id,
                        principalTable: "tbl_category_note",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_group_id = table.Column<int>(type: "int", nullable: false),
                    customer_code = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    customer_name_en = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    customer_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    customer_code_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    phone_1 = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    phone_2 = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    position = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    telegram = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
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
                name: "tbl_permission_option_role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false),
                    permission_option_id = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_permission_option_role", x => new { x.role_id, x.permission_option_id });
                    table.ForeignKey(
                        name: "FK_tbl_permission_option_role_tbl_permission_option_permission_option_id",
                        column: x => x.permission_option_id,
                        principalTable: "tbl_permission_option",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_permission_option_role_tbl_role_role_id",
                        column: x => x.role_id,
                        principalTable: "tbl_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, collation: "Khmer_100_BIN"),
                    full_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, collation: "Khmer_100_BIN"),
                    pin_code = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    phone_1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    phone_2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    is_allow_front_end_login = table.Column<bool>(type: "bit", nullable: false),
                    is_allow_backend_login = table.Column<bool>(type: "bit", nullable: false),
                    is_default = table.Column<bool>(type: "bit", nullable: false),
                    is_buildin = table.Column<bool>(type: "bit", nullable: false),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_tbl_role_role_id",
                        column: x => x.role_id,
                        principalTable: "tbl_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payment_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    payment_type_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    payment_type_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    is_build_in = table.Column<bool>(type: "bit", nullable: false),
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
                        name: "FK_tbl_payment_type_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_payment_type_tbl_currency_currency_id",
                        column: x => x.currency_id,
                        principalTable: "tbl_currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "tbl_station",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    station_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "Khmer_100_BIN"),
                    station_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_station", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_station_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_station_tbl_outlets_outlet_id",
                        column: x => x.outlet_id,
                        principalTable: "tbl_outlets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "tbl_project",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    project_type_id = table.Column<int>(type: "int", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    project_name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    start_date = table.Column<DateTime>(type: "date", nullable: true),
                    customer_code_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    closed_date = table.Column<DateTime>(type: "date", nullable: true),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    closed_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_paid = table.Column<bool>(type: "bit", nullable: false),
                    paid_date = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_tbl_project", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_project_tbl_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "tbl_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_project_tbl_project_type_project_type_id",
                        column: x => x.project_type_id,
                        principalTable: "tbl_project_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_attach_files",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    is_document_file = table.Column<bool>(type: "bit", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    project_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    file_title = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    file_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    file_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_extension = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_attach_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_attach_files_tbl_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "tbl_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_attach_files_tbl_project_project_id",
                        column: x => x.project_id,
                        principalTable: "tbl_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_contact",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    contact_name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    phone_1 = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    phone_2 = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    postion = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    email_address = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    telegram = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    project_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_contact", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_contact_tbl_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "tbl_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_contact_tbl_project_project_id",
                        column: x => x.project_id,
                        principalTable: "tbl_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    transaction_date = table.Column<DateTime>(type: "date", nullable: true),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    project_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    document_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    module = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    old_amount = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
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
                        name: "FK_tbl_history_tbl_project_project_id",
                        column: x => x.project_id,
                        principalTable: "tbl_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_history_tbl_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_business_branch_id",
                table: "Notes",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_category_note_id",
                table: "Notes",
                column: "category_note_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_customer_id",
                table: "tbl_attach_files",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_project_id",
                table: "tbl_attach_files",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_contact_customer_id",
                table: "tbl_contact",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_contact_project_id",
                table: "tbl_contact",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_currency_business_branch_id",
                table: "tbl_currency",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_customer_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_customer_id",
                table: "tbl_history",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_project_id",
                table: "tbl_history",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_user_id",
                table: "tbl_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_outlets_business_branch_id",
                table: "tbl_outlets",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_type_business_branch_id",
                table: "tbl_payment_type",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_type_currency_id",
                table: "tbl_payment_type",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_permission_option_parent_id",
                table: "tbl_permission_option",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_permission_option_role_permission_option_id",
                table: "tbl_permission_option_role",
                column: "permission_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_customer_id",
                table: "tbl_project",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_project_type_id",
                table: "tbl_project",
                column: "project_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_business_branch_id",
                table: "tbl_station",
                column: "business_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_outlet_id",
                table: "tbl_station",
                column: "outlet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_role_id",
                table: "tbl_user",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "StoreProcedureResultModel");

            migrationBuilder.DropTable(
                name: "tbl_attach_files");

            migrationBuilder.DropTable(
                name: "tbl_contact");

            migrationBuilder.DropTable(
                name: "tbl_country");

            migrationBuilder.DropTable(
                name: "tbl_document_number");

            migrationBuilder.DropTable(
                name: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_module_view");

            migrationBuilder.DropTable(
                name: "tbl_number");

            migrationBuilder.DropTable(
                name: "tbl_payment_type");

            migrationBuilder.DropTable(
                name: "tbl_permission_option_role");

            migrationBuilder.DropTable(
                name: "tbl_request_license");

            migrationBuilder.DropTable(
                name: "tbl_setting");

            migrationBuilder.DropTable(
                name: "tbl_station");

            migrationBuilder.DropTable(
                name: "tbl_category_note");

            migrationBuilder.DropTable(
                name: "tbl_project");

            migrationBuilder.DropTable(
                name: "tbl_user");

            migrationBuilder.DropTable(
                name: "tbl_currency");

            migrationBuilder.DropTable(
                name: "tbl_permission_option");

            migrationBuilder.DropTable(
                name: "tbl_outlets");

            migrationBuilder.DropTable(
                name: "tbl_customer");

            migrationBuilder.DropTable(
                name: "tbl_project_type");

            migrationBuilder.DropTable(
                name: "tbl_role");

            migrationBuilder.DropTable(
                name: "tbl_business_branch");

            migrationBuilder.DropTable(
                name: "tbl_customer_group");
        }
    }
}
