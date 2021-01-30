using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_tbl_note : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_tbl_business_branch_business_branch_id",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_tbl_category_note_category_note_id",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notes",
                table: "Notes");

            migrationBuilder.RenameTable(
                name: "Notes",
                newName: "tbl_note");

            migrationBuilder.RenameColumn(
                name: "note_label",
                table: "tbl_note",
                newName: "note");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_category_note_id",
                table: "tbl_note",
                newName: "IX_tbl_note_category_note_id");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_business_branch_id",
                table: "tbl_note",
                newName: "IX_tbl_note_business_branch_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_note",
                table: "tbl_note",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_note_tbl_business_branch_business_branch_id",
                table: "tbl_note",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_note_tbl_category_note_category_note_id",
                table: "tbl_note",
                column: "category_note_id",
                principalTable: "tbl_category_note",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_note_tbl_business_branch_business_branch_id",
                table: "tbl_note");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_note_tbl_category_note_category_note_id",
                table: "tbl_note");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_note",
                table: "tbl_note");

            migrationBuilder.RenameTable(
                name: "tbl_note",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "Notes",
                newName: "note_label");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_note_category_note_id",
                table: "Notes",
                newName: "IX_Notes_category_note_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_note_business_branch_id",
                table: "Notes",
                newName: "IX_Notes_business_branch_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notes",
                table: "Notes",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_tbl_business_branch_business_branch_id",
                table: "Notes",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_tbl_category_note_category_note_id",
                table: "Notes",
                column: "category_note_id",
                principalTable: "tbl_category_note",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
