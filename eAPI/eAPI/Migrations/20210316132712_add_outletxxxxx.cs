using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_outletxxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "outlet_id",
                table: "tbl_working_day",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "outlet_id",
                table: "tbl_payment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "outlet_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "outlet_id",
                table: "tbl_cashier_shift",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_cashier_shift");
        }
    }
}
