using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class sale_produc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_tbl_unit_unit_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "customer_note",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "due_date",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "is_fulfilled",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "is_over_due",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "reference_number",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "term_conditions",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "unit_id",
                table: "tbl_sale_product",
                newName: "status_id");

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

            migrationBuilder.RenameIndex(
                name: "IX_tbl_sale_product_unit_id",
                table: "tbl_sale_product",
                newName: "IX_tbl_sale_product_status_id");

            migrationBuilder.RenameColumn(
                name: "sale_date",
                table: "tbl_sale",
                newName: "working_date");

            migrationBuilder.AlterColumn<double>(
                name: "multiplier",
                table: "tbl_sale_product",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)");

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

            migrationBuilder.AddColumn<Guid>(
                name: "cashier_shift_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "working_day_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "tbl_sale_product_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    status_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    allow_send_to_printer = table.Column<bool>(type: "bit", nullable: false),
                    allow_append_quantity = table.Column<bool>(type: "bit", nullable: false),
                    allow_in_pos_order_list = table.Column<bool>(type: "bit", nullable: false),
                    allow_void_or_cancel_order = table.Column<bool>(type: "bit", nullable: false),
                    active_order = table.Column<bool>(type: "bit", nullable: false),
                    submited_status_id = table.Column<bool>(type: "bit", nullable: false),
                    allow_send_to_printer_when_change_table = table.Column<bool>(type: "bit", nullable: false),
                    allow_send_to_printer_when_merge_table = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale_product_status", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_tbl_sale_product_status_status_id",
                table: "tbl_sale_product",
                column: "status_id",
                principalTable: "tbl_sale_product_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_tbl_sale_product_status_status_id",
                table: "tbl_sale_product");

            migrationBuilder.DropTable(
                name: "tbl_sale_product_status");

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

            migrationBuilder.DropColumn(
                name: "cashier_shift_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "working_day_id",
                table: "tbl_sale");

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
                newName: "unit_id");

            migrationBuilder.RenameColumn(
                name: "is_free",
                table: "tbl_sale_product",
                newName: "is_fulfilled");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_sale_product_status_id",
                table: "tbl_sale_product",
                newName: "IX_tbl_sale_product_unit_id");

            migrationBuilder.RenameColumn(
                name: "working_date",
                table: "tbl_sale",
                newName: "sale_date");

            migrationBuilder.AlterColumn<decimal>(
                name: "multiplier",
                table: "tbl_sale_product",
                type: "decimal(19,4)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "customer_note",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "due_date",
                table: "tbl_sale",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_fulfilled",
                table: "tbl_sale",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_over_due",
                table: "tbl_sale",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "reference_number",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "term_conditions",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_tbl_unit_unit_id",
                table: "tbl_sale_product",
                column: "unit_id",
                principalTable: "tbl_unit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
