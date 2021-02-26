using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class modifier_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "modifier_category_id",
                table: "tbl_modifier",
                type: "int",
                nullable: true);

          

            migrationBuilder.CreateTable(
                name: "tbl_modifier_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modifier_category_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    is_multiple_select = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_modifier_category", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_modifier_category_id",
                table: "tbl_modifier",
                column: "modifier_category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_category_modifier_category_id",
                table: "tbl_modifier",
                column: "modifier_category_id",
                principalTable: "tbl_modifier_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_category_modifier_category_id",
                table: "tbl_modifier");

            migrationBuilder.DropTable(
                name: "tbl_modifier_category");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_modifier_category_id",
                table: "tbl_modifier");

            migrationBuilder.DropColumn(
                name: "modifier_category_id",
                table: "tbl_modifier");

         
        }
    }
}
