using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_outletxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "outlet_id",
                table: "tbl_table_group",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "outlet_id",
                table: "tbl_station",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "outlet_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "tbl_outlet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    outlet_name_en = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "Khmer_100_BIN"),
                    outlet_name_kh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_outlet", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_outlet_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_outlet_station",
                columns: table => new
                {
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    station_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_outlet_station", x => new { x.station_id, x.outlet_id });
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_table_group_outlet_id",
                table: "tbl_table_group",
                column: "outlet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_outlet_id",
                table: "tbl_station",
                column: "outlet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_outlet_id",
                table: "tbl_sale",
                column: "outlet_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_outlet_business_branch_id",
                table: "tbl_outlet",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_outlet_outlet_id",
                table: "tbl_sale",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_station_tbl_outlet_outlet_id",
                table: "tbl_station",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_table_group_tbl_outlet_outlet_id",
                table: "tbl_table_group",
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

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_table_group_tbl_outlet_outlet_id",
                table: "tbl_table_group");

            migrationBuilder.DropTable(
                name: "tbl_outlet");

            migrationBuilder.DropTable(
                name: "tbl_outlet_station");

            migrationBuilder.DropIndex(
                name: "IX_tbl_table_group_outlet_id",
                table: "tbl_table_group");

            migrationBuilder.DropIndex(
                name: "IX_tbl_station_outlet_id",
                table: "tbl_station");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_table_group");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_sale");
        }
    }
}
