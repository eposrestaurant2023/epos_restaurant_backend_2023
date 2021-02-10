using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class qqqqq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_OutletModel_outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_station_OutletModel_outlet_id",
                table: "tbl_station");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OutletModel",
                table: "OutletModel");

            migrationBuilder.RenameTable(
                name: "OutletModel",
                newName: "tbl_outlet");

            migrationBuilder.RenameColumn(
                name: "tableid",
                table: "tbl_user",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "tableid",
                table: "tbl_station",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "tableid",
                table: "tbl_payment_type",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "tableid",
                table: "tbl_outlet",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_outlet",
                table: "tbl_outlet",
                column: "id");

            migrationBuilder.CreateTable(
                name: "tbl_setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    setting_title = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    setting_description = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    setting_value = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_setting", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_table_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    outlet_id = table.Column<int>(type: "int", nullable: false),
                    table_group_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    table_group_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_table_group", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_table_group_tbl_outlet_outlet_id",
                        column: x => x.outlet_id,
                        principalTable: "tbl_outlet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_table",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    table_group_id = table.Column<int>(type: "int", nullable: false),
                    table_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    position_x_percent = table.Column<double>(type: "float", nullable: false),
                    position_y_percent = table.Column<double>(type: "float", nullable: false),
                    height = table.Column<double>(type: "float", nullable: false),
                    width = table.Column<double>(type: "float", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_table", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_table_tbl_table_group_table_group_id",
                        column: x => x.table_group_id,
                        principalTable: "tbl_table_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_table_table_group_id",
                table: "tbl_table",
                column: "table_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_table_group_outlet_id",
                table: "tbl_table_group",
                column: "outlet_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_outlet_outlet_id",
                table: "tbl_sale",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_station_tbl_outlet_outlet_id",
                table: "tbl_station",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_outlet_outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_station_tbl_outlet_outlet_id",
                table: "tbl_station");

            migrationBuilder.DropTable(
                name: "tbl_setting");

            migrationBuilder.DropTable(
                name: "tbl_table");

            migrationBuilder.DropTable(
                name: "tbl_table_group");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_outlet",
                table: "tbl_outlet");

            migrationBuilder.RenameTable(
                name: "tbl_outlet",
                newName: "OutletModel");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_user",
                newName: "tableid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_station",
                newName: "tableid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_payment_type",
                newName: "tableid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "OutletModel",
                newName: "tableid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutletModel",
                table: "OutletModel",
                column: "tableid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_OutletModel_outlet_id",
                table: "tbl_sale",
                column: "outlet_id",
                principalTable: "OutletModel",
                principalColumn: "tableid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_station_OutletModel_outlet_id",
                table: "tbl_station",
                column: "outlet_id",
                principalTable: "OutletModel",
                principalColumn: "tableid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
