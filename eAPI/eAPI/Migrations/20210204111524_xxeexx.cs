﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class xxeexx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "tbl_config_data",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "note",
                table: "tbl_config_data");
        }
    }
}