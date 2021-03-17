using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class dee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "tbl_sale_product_print_queue",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "outlet_name",
                table: "tbl_sale_product_print_queue",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "printer_ip_address",
                table: "tbl_sale_product_print_queue",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "printer_name",
                table: "tbl_sale_product_print_queue",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "printer_port",
                table: "tbl_sale_product_print_queue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "station_name",
                table: "tbl_sale_product_print_queue",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "outlet_name",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.DropColumn(
                name: "printer_ip_address",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.DropColumn(
                name: "printer_name",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.DropColumn(
                name: "printer_port",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.DropColumn(
                name: "station_name",
                table: "tbl_sale_product_print_queue");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "tbl_sale_product_print_queue",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_sale_product_print_queue",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_sale_product_print_queue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tbl_sale_product_print_queue",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_sale_product_print_queue",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
