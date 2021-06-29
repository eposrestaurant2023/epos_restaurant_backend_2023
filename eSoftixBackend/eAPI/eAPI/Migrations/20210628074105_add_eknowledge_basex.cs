using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_eknowledge_basex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_eknowledge_base_tbl_eknowledge_base_parent_id",
                table: "tbl_eknowledge_base");

            migrationBuilder.AlterColumn<Guid>(
                name: "parent_id",
                table: "tbl_eknowledge_base",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_eknowledge_base_tbl_eknowledge_base_parent_id",
                table: "tbl_eknowledge_base",
                column: "parent_id",
                principalTable: "tbl_eknowledge_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_eknowledge_base_tbl_eknowledge_base_parent_id",
                table: "tbl_eknowledge_base");

            migrationBuilder.AlterColumn<Guid>(
                name: "parent_id",
                table: "tbl_eknowledge_base",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_eknowledge_base_tbl_eknowledge_base_parent_id",
                table: "tbl_eknowledge_base",
                column: "parent_id",
                principalTable: "tbl_eknowledge_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
