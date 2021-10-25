using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class currenct_1dxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "closed_outlet_id",
                table: "tbl_sale");

            migrationBuilder.AddColumn<string>(
                name: "cash_drawer_name",
                table: "tbl_working_day",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<Guid>(
                name: "cash_drawer_id",
                table: "tbl_cashier_shift",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "cash_drawer_name",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cash_drawer_name",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "cash_drawer_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "cash_drawer_name",
                table: "tbl_cashier_shift");

            migrationBuilder.AddColumn<Guid>(
                name: "closed_outlet_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
