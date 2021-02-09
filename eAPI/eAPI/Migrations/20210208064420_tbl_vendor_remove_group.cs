using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class tbl_vendor_remove_group : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_vendor_tbl_vendor_group_VendorGroupModelid",
                table: "tbl_vendor");

            migrationBuilder.DropIndex(
                name: "IX_tbl_vendor_VendorGroupModelid",
                table: "tbl_vendor");

            migrationBuilder.DropColumn(
                name: "VendorGroupModelid",
                table: "tbl_vendor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorGroupModelid",
                table: "tbl_vendor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_vendor_VendorGroupModelid",
                table: "tbl_vendor",
                column: "VendorGroupModelid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_vendor_tbl_vendor_group_VendorGroupModelid",
                table: "tbl_vendor",
                column: "VendorGroupModelid",
                principalTable: "tbl_vendor_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
