﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class attifle_eknoledgebase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "eknowledgebase_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_eknowledgebase_id",
                table: "tbl_attach_files",
                column: "eknowledgebase_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_eknowledge_base_eknowledgebase_id",
                table: "tbl_attach_files",
                column: "eknowledgebase_id",
                principalTable: "tbl_eknowledge_base",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_eknowledge_base_eknowledgebase_id",
                table: "tbl_attach_files");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_eknowledgebase_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "eknowledgebase_id",
                table: "tbl_attach_files");
        }
    }
}
