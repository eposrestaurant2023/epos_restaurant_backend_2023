﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class ad_allow_append_qty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "allow_append_quantity",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_append_quantity",
                table: "tbl_product");
        }
    }
}