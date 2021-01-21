using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class test_guid_insertxxxxxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_customer_id",
                table: "tbl_history",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_customer_id",
                table: "tbl_attach_files",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_customer_customer_id",
                table: "tbl_attach_files",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_customer_customer_id",
                table: "tbl_history",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_customer_customer_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_customer_customer_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_customer_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_customer_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "tbl_attach_files");
        }
    }
}
