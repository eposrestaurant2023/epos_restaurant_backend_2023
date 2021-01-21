using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_customer_groupxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_outlets_tbl_business_branch_business_branch_id",
                table: "outlets");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_outlet_station_outlets_outlet_id",
                table: "tbl_outlet_station");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_table_group_screen_outlets_outlet_id",
                table: "tbl_table_group_screen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_outlets",
                table: "outlets");

            migrationBuilder.RenameTable(
                name: "outlets",
                newName: "tbl_outlet");

            migrationBuilder.RenameIndex(
                name: "IX_outlets_business_branch_id",
                table: "tbl_outlet",
                newName: "IX_tbl_outlet_business_branch_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_outlet",
                table: "tbl_outlet",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_outlet_tbl_business_branch_business_branch_id",
                table: "tbl_outlet",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_outlet_station_tbl_outlet_outlet_id",
                table: "tbl_outlet_station",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_table_group_screen_tbl_outlet_outlet_id",
                table: "tbl_table_group_screen",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_outlet_tbl_business_branch_business_branch_id",
                table: "tbl_outlet");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_outlet_station_tbl_outlet_outlet_id",
                table: "tbl_outlet_station");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_table_group_screen_tbl_outlet_outlet_id",
                table: "tbl_table_group_screen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_outlet",
                table: "tbl_outlet");

            migrationBuilder.RenameTable(
                name: "tbl_outlet",
                newName: "outlets");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_outlet_business_branch_id",
                table: "outlets",
                newName: "IX_outlets_business_branch_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outlets",
                table: "outlets",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_outlets_tbl_business_branch_business_branch_id",
                table: "outlets",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_outlet_station_outlets_outlet_id",
                table: "tbl_outlet_station",
                column: "outlet_id",
                principalTable: "outlets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_table_group_screen_outlets_outlet_id",
                table: "tbl_table_group_screen",
                column: "outlet_id",
                principalTable: "outlets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
