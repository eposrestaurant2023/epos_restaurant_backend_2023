using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_printer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_printer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ip_address",
                table: "tbl_printer",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "port",
                table: "tbl_printer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_printer_business_branch_id",
                table: "tbl_printer",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_printer_tbl_business_branch_business_branch_id",
                table: "tbl_printer",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_printer_tbl_business_branch_business_branch_id",
                table: "tbl_printer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_printer_business_branch_id",
                table: "tbl_printer");

            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_printer");

            migrationBuilder.DropColumn(
                name: "ip_address",
                table: "tbl_printer");

            migrationBuilder.DropColumn(
                name: "port",
                table: "tbl_printer");
        }
    }
}
