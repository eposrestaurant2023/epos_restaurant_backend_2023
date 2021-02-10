using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class bbbb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_SaleProductShareModel_id",
                table: "tbl_sale_product");

            migrationBuilder.DropTable(
                name: "SaleProductShareModel");

            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_sale_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_sale_product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_sale_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_sale_product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "discount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "discount_type",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "grand_total",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "invoice_discount_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "is_allow_discount",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_fulfilled",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_inventory_product",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "multiplier",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "quantity",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "regular_price",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "sale_product_note",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "selling_price",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "sub_total",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_discount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "discount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "discount_type",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "grand_total",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "invoice_discount_amount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "is_allow_discount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "is_fulfilled",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "is_inventory_product",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "regular_price",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "sale_product_note",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "selling_price",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "sub_total",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "total_discount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_sale_product");

            migrationBuilder.CreateTable(
                name: "SaleProductShareModel",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SaleModelid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    cost = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    discount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    discount_type = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    grand_total = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    invoice_discount_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    is_allow_discount = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    is_fulfilled = table.Column<bool>(type: "bit", nullable: false),
                    is_inventory_product = table.Column<bool>(type: "bit", nullable: false),
                    multiplier = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    regular_price = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    sale_product_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    selling_price = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    total_discount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
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

            migrationBuilder.CreateIndex(
                name: "IX_SaleProductShareModel_SaleModelid",
                table: "SaleProductShareModel",
                column: "SaleModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_SaleProductShareModel_id",
                table: "tbl_sale_product",
                column: "id",
                principalTable: "SaleProductShareModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
