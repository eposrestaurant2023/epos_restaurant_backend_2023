using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class xxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "other",
                table: "tbl_business_branch",
                newName: "note");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessBranchModelid",
                table: "tbl_contact",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "tbl_business_branch",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.CreateTable(
                name: "tbl_stock_location",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stock_location_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_default = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_stock_location", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_stock_location_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_contact_BusinessBranchModelid",
                table: "tbl_contact",
                column: "BusinessBranchModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_business_branch_id",
                table: "tbl_stock_location",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_contact_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_contact",
                column: "BusinessBranchModelid",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_contact_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_contact");

            migrationBuilder.DropTable(
                name: "tbl_stock_location");

            migrationBuilder.DropIndex(
                name: "IX_tbl_contact_BusinessBranchModelid",
                table: "tbl_contact");

            migrationBuilder.DropColumn(
                name: "BusinessBranchModelid",
                table: "tbl_contact");

            migrationBuilder.DropColumn(
                name: "color",
                table: "tbl_business_branch");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "tbl_business_branch",
                newName: "other");
        }
    }
}
