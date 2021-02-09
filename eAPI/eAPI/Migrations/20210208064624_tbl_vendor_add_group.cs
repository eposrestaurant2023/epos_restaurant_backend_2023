using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class tbl_vendor_add_group : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "vendor_group_id",
                table: "tbl_vendor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_vendor_vendor_group_id",
                table: "tbl_vendor",
                column: "vendor_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_vendor_tbl_vendor_group_vendor_group_id",
                table: "tbl_vendor",
                column: "vendor_group_id",
                principalTable: "tbl_vendor_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_vendor_tbl_vendor_group_vendor_group_id",
                table: "tbl_vendor");

            migrationBuilder.DropIndex(
                name: "IX_tbl_vendor_vendor_group_id",
                table: "tbl_vendor");

            migrationBuilder.DropColumn(
                name: "vendor_group_id",
                table: "tbl_vendor");
        }
    }
}
