﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_attach_file_to_expense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "expense_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseModelid",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_expense_id",
                table: "tbl_history",
                column: "expense_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_ExpenseModelid",
                table: "tbl_attach_files",
                column: "ExpenseModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_expense_ExpenseModelid",
                table: "tbl_attach_files",
                column: "ExpenseModelid",
                principalTable: "tbl_expense",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_expense_expense_id",
                table: "tbl_history",
                column: "expense_id",
                principalTable: "tbl_expense",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_expense_ExpenseModelid",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_expense_expense_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_expense_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_ExpenseModelid",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "expense_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "ExpenseModelid",
                table: "tbl_attach_files");
        }
    }
}
