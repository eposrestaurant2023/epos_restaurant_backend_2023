using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class currenct_1dxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "cash_drawer_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "closed_cash_drawer_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "closed_cashier_shift_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "closed_outlet_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "closed_station_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "closed_working_day_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cash_drawer_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_cash_drawer_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_cashier_shift_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_station_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "closed_working_day_id",
                table: "tbl_sale");
        }
    }
}
