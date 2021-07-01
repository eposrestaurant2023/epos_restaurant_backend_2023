using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_taxt_to_tbl_sation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tax_1_name",
                table: "tbl_station",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_1_taxable_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "tax_2_name",
                table: "tbl_station",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_2_taxable_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "tax_3_name",
                table: "tbl_station",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "tax_3_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_3_taxable_rate",
                table: "tbl_station",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
             
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_working_day_tbl_outlet_outlet_id",
                table: "tbl_working_day");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_working_day_tbl_station_closed_station_id",
                table: "tbl_working_day");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_working_day_tbl_station_opened_station_id",
                table: "tbl_working_day");

            migrationBuilder.DropIndex(
                name: "IX_tbl_working_day_closed_station_id",
                table: "tbl_working_day");

            migrationBuilder.DropIndex(
                name: "IX_tbl_working_day_opened_station_id",
                table: "tbl_working_day");

            migrationBuilder.DropIndex(
                name: "IX_tbl_working_day_outlet_id",
                table: "tbl_working_day");

            migrationBuilder.DropColumn(
                name: "tax_1_name",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_1_rate",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_1_taxable_rate",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_2_name",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_2_rate",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_2_taxable_rate",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_3_name",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_3_rate",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "tax_3_taxable_rate",
                table: "tbl_station");
        }
    }
}
