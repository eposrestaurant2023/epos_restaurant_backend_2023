using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class delete_outlet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_outlet_outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_table_group_tbl_outlet_outlet_id",
                table: "tbl_table_group");

            migrationBuilder.DropTable(
                name: "tbl_outlet");

            migrationBuilder.DropIndex(
                name: "IX_tbl_table_group_outlet_id",
                table: "tbl_table_group");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_outlet_id",
                table: "tbl_sale");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_table_group");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "outlet_id",
                table: "tbl_table_group",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbl_outlet",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    outlet_name_en = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "Khmer_100_BIN"),
                    outlet_name_kh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
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

            migrationBuilder.CreateIndex(
                name: "IX_tbl_table_group_outlet_id",
                table: "tbl_table_group",
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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_table_group_tbl_outlet_outlet_id",
                table: "tbl_table_group",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
