﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "product_category_name_en",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_category_name_kh",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "product_group_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "product_group_name_en",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_group_name_kh",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_category_name_en",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_category_name_kh",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_name_en",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_group_name_kh",
                table: "tbl_product");
        }
    }
}