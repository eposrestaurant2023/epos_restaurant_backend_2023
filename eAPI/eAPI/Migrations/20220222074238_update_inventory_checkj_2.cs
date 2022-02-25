using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_inventory_checkj_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_check_product_tbl_inventory_check_InventoryCheckModelid",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_check_product_InventoryCheckModelid",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "InventoryCheckModelid",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "tbl_inventory_check_product");

            migrationBuilder.AddColumn<Guid>(
                name: "inventory_check_id",
                table: "tbl_inventory_check_product",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "multiplier",
                table: "tbl_inventory_check_product",
                type: "decimal(19,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "tbl_inventory_check_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_check_product_inventory_check_id",
                table: "tbl_inventory_check_product",
                column: "inventory_check_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_check_product_tbl_inventory_check_inventory_check_id",
                table: "tbl_inventory_check_product",
                column: "inventory_check_id",
                principalTable: "tbl_inventory_check",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_inventory_check_product_tbl_inventory_check_inventory_check_id",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_inventory_check_product_inventory_check_id",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "inventory_check_id",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_inventory_check_product");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "tbl_inventory_check_product");

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryCheckModelid",
                table: "tbl_inventory_check_product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tbl_inventory_check_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tbl_inventory_check_product",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "tbl_inventory_check_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "tbl_inventory_check_product",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_inventory_check_product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_inventory_check_product",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tbl_inventory_check_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "tbl_inventory_check_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_inventory_check_product_InventoryCheckModelid",
                table: "tbl_inventory_check_product",
                column: "InventoryCheckModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_inventory_check_product_tbl_inventory_check_InventoryCheckModelid",
                table: "tbl_inventory_check_product",
                column: "InventoryCheckModelid",
                principalTable: "tbl_inventory_check",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
