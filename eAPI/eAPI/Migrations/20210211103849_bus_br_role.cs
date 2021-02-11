using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class bus_br_role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_business_branch_role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_delete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_business_branch_role", x => new { x.role_id, x.business_branch_id });
                    table.ForeignKey(
                        name: "FK_tbl_business_branch_role_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_business_branch_role_tbl_role_role_id",
                        column: x => x.role_id,
                        principalTable: "tbl_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_role_business_branch_id",
                table: "tbl_business_branch_role",
                column: "business_branch_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_business_branch_role");
        }
    }
}
