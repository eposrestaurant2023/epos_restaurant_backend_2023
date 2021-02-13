using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sjkdjll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_shift_shift_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_shift",
                table: "tbl_shift");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tbl_shift");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_shift",
                table: "tbl_shift",
                column: "sid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_shift_shift_id",
                table: "tbl_cashier_shift",
                column: "shift_id",
                principalTable: "tbl_shift",
                principalColumn: "sid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_shift_shift_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_shift",
                table: "tbl_shift");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "tbl_shift",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_shift",
                table: "tbl_shift",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_shift_shift_id",
                table: "tbl_cashier_shift",
                column: "shift_id",
                principalTable: "tbl_shift",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
