using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class product_group : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_prefix_price_tbl_payment_type_payment_typeid",
                table: "tbl_prefix_price");

            migrationBuilder.DropTable(
                name: "tbl_payment_type");

            migrationBuilder.DropTable(
                name: "tbl_currency");

            migrationBuilder.DropIndex(
                name: "IX_tbl_prefix_price_payment_typeid",
                table: "tbl_prefix_price");

            migrationBuilder.DropColumn(
                name: "payment_typeid",
                table: "tbl_prefix_price");

            migrationBuilder.AddColumn<string>(
                name: "product_category_en",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "product_category_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "product_category_kh",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_group_code",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_group_en",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "product_group_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "product_group_kh",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_category_en",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_category_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_category_kh",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_code",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_en",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_kh",
                table: "tbl_product");

            migrationBuilder.AddColumn<int>(
                name: "payment_typeid",
                table: "tbl_prefix_price",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_currency",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    change_exchange_rate = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    currency_format = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payment_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: true),
                    deleted_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_build_in = table.Column<bool>(type: "bit", nullable: false),
                    is_credit = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_tbl_prefix_price_payment_typeid",
                table: "tbl_prefix_price",
                column: "payment_typeid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_type_currency_id",
                table: "tbl_payment_type",
                column: "currency_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_prefix_price_tbl_payment_type_payment_typeid",
                table: "tbl_prefix_price",
                column: "payment_typeid",
                principalTable: "tbl_payment_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
