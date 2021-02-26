using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class change_sale_proeuct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_customer_customer_id",
                table: "tbl_sale");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_tbl_product_type_product_type_id",
                table: "tbl_sale_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_product_product_type_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "grand_total_discount",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "selling_price",
                table: "tbl_sale_product",
                newName: "total_revenue");

            migrationBuilder.RenameColumn(
                name: "sale_product_note",
                table: "tbl_sale_product",
                newName: "unit");

            migrationBuilder.RenameColumn(
                name: "regular_price",
                table: "tbl_sale_product",
                newName: "total_modifier_amount");

            migrationBuilder.RenameColumn(
                name: "product_type_id",
                table: "tbl_sale_product",
                newName: "status_id");

            migrationBuilder.RenameColumn(
                name: "is_fulfilled",
                table: "tbl_sale_product",
                newName: "is_free");

            migrationBuilder.RenameColumn(
                name: "invoice_discount_amount",
                table: "tbl_sale_product",
                newName: "taxable_amount");

            migrationBuilder.RenameColumn(
                name: "grand_total",
                table: "tbl_sale_product",
                newName: "tax_3_taxable_amount");

            migrationBuilder.AddColumn<string>(
                name: "deleted_note",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "discount_lable",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "discount_note",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "free_note",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<bool>(
                name: "is_allow_free",
                table: "tbl_sale_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "kitchen_group_name",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "kitchen_group_sort_order",
                table: "tbl_sale_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "multiplier",
                table: "tbl_sale_product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "portion_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "portion_name",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "reqular_price",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "status_name",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_taxable_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_taxable_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_3_amount",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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
                name: "IX_tbl_sale_product_modifier_sale_product_id",
                table: "tbl_sale_product_modifier",
                column: "sale_product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_customer_customer_id",
                table: "tbl_sale",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_customer_customer_id",
                table: "tbl_sale");

            migrationBuilder.DropTable(
                name: "tbl_sale_product_modifier");

            migrationBuilder.DropColumn(
                name: "deleted_note",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "discount_lable",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "discount_note",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "free_note",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "is_allow_free",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "kitchen_group_name",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "kitchen_group_sort_order",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "note",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "portion_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "portion_name",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "price",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "reqular_price",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "status_name",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "tax_1_amount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "tax_1_rate",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "tax_1_taxable_amount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "tax_2_amount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "tax_2_rate",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "tax_2_taxable_amount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "tax_3_amount",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "tax_3_rate",
                table: "tbl_sale_product");

            migrationBuilder.RenameColumn(
                name: "unit",
                table: "tbl_sale_product",
                newName: "sale_product_note");

            migrationBuilder.RenameColumn(
                name: "total_revenue",
                table: "tbl_sale_product",
                newName: "selling_price");

            migrationBuilder.RenameColumn(
                name: "total_modifier_amount",
                table: "tbl_sale_product",
                newName: "regular_price");

            migrationBuilder.RenameColumn(
                name: "taxable_amount",
                table: "tbl_sale_product",
                newName: "invoice_discount_amount");

            migrationBuilder.RenameColumn(
                name: "tax_3_taxable_amount",
                table: "tbl_sale_product",
                newName: "grand_total");

            migrationBuilder.RenameColumn(
                name: "status_id",
                table: "tbl_sale_product",
                newName: "product_type_id");

            migrationBuilder.RenameColumn(
                name: "is_free",
                table: "tbl_sale_product",
                newName: "is_fulfilled");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<decimal>(
                name: "grand_total_discount",
                table: "tbl_sale",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_product_type_id",
                table: "tbl_sale_product",
                column: "product_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_customer_customer_id",
                table: "tbl_sale",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_tbl_product_type_product_type_id",
                table: "tbl_sale_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
