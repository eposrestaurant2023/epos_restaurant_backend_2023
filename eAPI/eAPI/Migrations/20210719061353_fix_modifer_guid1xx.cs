using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_modifer_guid1xx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "modifier_group_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_modifier_group_id",
                table: "tbl_attach_files",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_modifier_id",
                table: "tbl_attach_files",
                column: "modifier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_group_modifier_group_id",
                table: "tbl_attach_files",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_modifier_id",
                table: "tbl_attach_files",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_group_modifier_group_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_modifier_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_modifier_group_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_modifier_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "modifier_id",
                table: "tbl_attach_files");
        }
    }
}
