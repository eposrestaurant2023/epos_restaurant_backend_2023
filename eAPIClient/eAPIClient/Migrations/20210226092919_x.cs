using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class x : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "is_new_customer",
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
                name: "sale_date",
                table: "tbl_sale",
                newName: "working_date");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_sale_product_status");

            migrationBuilder.DropColumn(
                name: "cashier_shift_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "working_day_id",
                table: "tbl_sale");

            migrationBuilder.RenameColumn(
                name: "working_date",
                table: "tbl_sale",
                newName: "sale_date");

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
                name: "is_new_customer",
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
        }
    }
}
