using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_system_feature3x : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_system_feature_tbl_business_branch_business_branch_id",
                table: "tbl_business_branch_system_feature",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_project_system_feature_tbl_project_project_id",
                table: "tbl_project_system_feature",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_system_feature_tbl_business_branch_business_branch_id",
                table: "tbl_business_branch_system_feature");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_project_system_feature_tbl_project_project_id",
                table: "tbl_project_system_feature");
        }
    }
}
