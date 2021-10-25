using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_cash_drawer_table2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "cash_drawer_id",
                table: "tbl_working_day",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "cash_drawer_name",
                table: "tbl_station",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cash_drawer_id",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "cash_drawer_name",
                table: "tbl_station");
        }
    }
}
