using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_system_feature3x4x : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_system_feature_system_feature_id",
                table: "tbl_project_system_feature",
                column: "system_feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_system_feature_system_feature_id",
                table: "tbl_business_branch_system_feature",
                column: "system_feature_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_system_feature_tbl_system_feature_system_feature_id",
                table: "tbl_business_branch_system_feature",
                column: "system_feature_id",
                principalTable: "tbl_system_feature",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_project_system_feature_tbl_system_feature_system_feature_id",
                table: "tbl_project_system_feature",
                column: "system_feature_id",
                principalTable: "tbl_system_feature",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_system_feature_tbl_system_feature_system_feature_id",
                table: "tbl_business_branch_system_feature");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_project_system_feature_tbl_system_feature_system_feature_id",
                table: "tbl_project_system_feature");

            migrationBuilder.DropIndex(
                name: "IX_tbl_project_system_feature_system_feature_id",
                table: "tbl_project_system_feature");

            migrationBuilder.DropIndex(
                name: "IX_tbl_business_branch_system_feature_system_feature_id",
                table: "tbl_business_branch_system_feature");
        }
    }
}
