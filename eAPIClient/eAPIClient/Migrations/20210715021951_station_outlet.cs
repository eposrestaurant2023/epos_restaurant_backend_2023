using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class station_outlet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "station_id",
                table: "tbl_sale_product",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "station_name_en",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "station_name_kh",
                table: "tbl_sale_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "outlet_name_en",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "outlet_name_kh",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "station_name_en",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "station_name_kh",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "station_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "station_name_en",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "station_name_kh",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "outlet_name_en",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "outlet_name_kh",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "station_name_en",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "station_name_kh",
                table: "tbl_sale");
        }
    }
}
