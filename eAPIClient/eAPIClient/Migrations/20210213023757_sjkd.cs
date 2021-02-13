using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sjkd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashierShiftShareModel_ShiftModel_shift_id",
                table: "CashierShiftShareModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftModel",
                table: "ShiftModel");

            migrationBuilder.RenameTable(
                name: "ShiftModel",
                newName: "tbl_shift");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_shift",
                table: "tbl_shift",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashierShiftShareModel_tbl_shift_shift_id",
                table: "CashierShiftShareModel",
                column: "shift_id",
                principalTable: "tbl_shift",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashierShiftShareModel_tbl_shift_shift_id",
                table: "CashierShiftShareModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_shift",
                table: "tbl_shift");

            migrationBuilder.RenameTable(
                name: "tbl_shift",
                newName: "ShiftModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftModel",
                table: "ShiftModel",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashierShiftShareModel_ShiftModel_shift_id",
                table: "CashierShiftShareModel",
                column: "shift_id",
                principalTable: "ShiftModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
