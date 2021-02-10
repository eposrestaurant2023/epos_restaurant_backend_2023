﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class abkjgh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group");



            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group",
                column: "groupid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id",
                principalTable: "tbl_customer_group",
                principalColumn: "groupid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group");



            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id",
                principalTable: "tbl_customer_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
