using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_bus_settiing_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_business_branch_setting",
                columns: table => new
                {
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    setting_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_business_branch_setting", x => new { x.setting_id, x.business_branch_id });
                    table.ForeignKey(
                        name: "FK_tbl_business_branch_setting_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_business_branch_setting_tbl_setting_setting_id",
                        column: x => x.setting_id,
                        principalTable: "tbl_setting",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_setting_business_branch_id",
                table: "tbl_business_branch_setting",
                column: "business_branch_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_business_branch_setting");
        }
    }
}
