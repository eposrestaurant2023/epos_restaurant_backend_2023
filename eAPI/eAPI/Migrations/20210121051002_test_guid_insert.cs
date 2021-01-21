using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class test_guid_insert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "other",
                table: "tbl_business_branch",
                newName: "note");

            migrationBuilder.AddColumn<string>(
                name: "customer_code",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "photo",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.CreateTable(
                name: "tbl_main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    main_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_main", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sub",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    main_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sub_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sub", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sub_tbl_main_main_id",
                        column: x => x.main_id,
                        principalTable: "tbl_main",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sub_main_id",
                table: "tbl_sub",
                column: "main_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_sub");

            migrationBuilder.DropTable(
                name: "tbl_main");

            migrationBuilder.DropColumn(
                name: "customer_code",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "photo",
                table: "tbl_customer");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "tbl_business_branch",
                newName: "other");
        }
    }
}
