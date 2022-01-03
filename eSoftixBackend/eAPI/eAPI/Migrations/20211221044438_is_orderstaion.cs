﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class is_orderstaion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_order_station",
                table: "tbl_station",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_order_station",
                table: "tbl_station");
        }
    }
}