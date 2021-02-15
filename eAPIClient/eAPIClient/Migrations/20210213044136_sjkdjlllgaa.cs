using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sjkdjlllgaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_shift_shift_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cashier_shift_shift_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropColumn(
                name: "shift_id",
                table: "tbl_cashier_shift");

            migrationBuilder.AddColumn<string>(
                name: "shift",
                table: "tbl_cashier_shift",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shift",
                table: "tbl_cashier_shift");

            migrationBuilder.AddColumn<int>(
                name: "shift_id",
                table: "tbl_cashier_shift",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cashier_shift_shift_id",
                table: "tbl_cashier_shift",
                column: "shift_id");

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
