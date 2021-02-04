using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class xaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_customer_customer_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_product_product_id",
                table: "tbl_attach_files");

            migrationBuilder.AlterColumn<int>(
                name: "product_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_customer_customer_id",
                table: "tbl_attach_files",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_product_product_id",
                table: "tbl_attach_files",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_customer_customer_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_product_product_id",
                table: "tbl_attach_files");

            migrationBuilder.AlterColumn<int>(
                name: "product_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_customer_customer_id",
                table: "tbl_attach_files",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_product_product_id",
                table: "tbl_attach_files",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
