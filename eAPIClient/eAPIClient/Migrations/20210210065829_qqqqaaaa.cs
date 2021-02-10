using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class qqqqaaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_table_group",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_sale",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "payment_type_id",
                table: "tbl_payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbl_currency",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    currency_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    currency_format = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    change_exchange_rate = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_outlet",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    outlet_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    outlet_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_outlet", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payment_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    payment_type_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_payment_type_tbl_currency_currency_id",
                        column: x => x.currency_id,
                        principalTable: "tbl_currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_station",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
                    station_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    station_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_already_config = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_station", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_station_tbl_outlet_outlet_id",
                        column: x => x.outlet_id,
                        principalTable: "tbl_outlet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_table_group_outlet_id",
                table: "tbl_table_group",
                column: "outlet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_outlet_id",
                table: "tbl_sale",
                column: "outlet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_payment_type_id",
                table: "tbl_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_type_currency_id",
                table: "tbl_payment_type",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_outlet_id",
                table: "tbl_station",
                column: "outlet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_tbl_payment_type_payment_type_id",
                table: "tbl_payment",
                column: "payment_type_id",
                principalTable: "tbl_payment_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_outlet_outlet_id",
                table: "tbl_sale",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_table_group_tbl_outlet_outlet_id",
                table: "tbl_table_group",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_tbl_payment_type_payment_type_id",
                table: "tbl_payment");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_outlet_outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_table_group_tbl_outlet_outlet_id",
                table: "tbl_table_group");

            migrationBuilder.DropTable(
                name: "tbl_payment_type");

            migrationBuilder.DropTable(
                name: "tbl_station");

            migrationBuilder.DropTable(
                name: "tbl_currency");

            migrationBuilder.DropTable(
                name: "tbl_outlet");

            migrationBuilder.DropIndex(
                name: "IX_tbl_table_group_outlet_id",
                table: "tbl_table_group");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropIndex(
                name: "IX_tbl_payment_payment_type_id",
                table: "tbl_payment");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_table_group");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "payment_type_id",
                table: "tbl_payment");
        }
    }
}
