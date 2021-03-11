using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class tbl_project_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "tbl_contact",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbl_project_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_project_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_project",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_type_id = table.Column<int>(type: "int", nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    project_name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Khmer_100_BIN"),
                    start_date = table.Column<DateTime>(type: "date", nullable: true),
                    customer_code_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    closed_date = table.Column<DateTime>(type: "date", nullable: true),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    closed_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_paid = table.Column<bool>(type: "bit", nullable: false),
                    paid_date = table.Column<DateTime>(type: "date", nullable: true),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_project", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_project_tbl_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "tbl_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_project_tbl_project_type_project_type_id",
                        column: x => x.project_type_id,
                        principalTable: "tbl_project_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_contact_project_id",
                table: "tbl_contact",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_customer_id",
                table: "tbl_project",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_project_type_id",
                table: "tbl_project",
                column: "project_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_contact_tbl_project_project_id",
                table: "tbl_contact",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_contact_tbl_project_project_id",
                table: "tbl_contact");

            migrationBuilder.DropTable(
                name: "tbl_project");

            migrationBuilder.DropTable(
                name: "tbl_project_type");

            migrationBuilder.DropIndex(
                name: "IX_tbl_contact_project_id",
                table: "tbl_contact");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "tbl_contact");
        }
    }
}
