using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_config_dataxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_user_business_branch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_user_business_branch",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_business_branch", x => new { x.user_id, x.business_branch_id });
                    table.ForeignKey(
                        name: "FK_tbl_user_business_branch_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_user_business_branch_tbl_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_business_branch_business_branch_id",
                table: "tbl_user_business_branch",
                column: "business_branch_id");
        }
    }
}
