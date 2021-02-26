using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_parent_child_modi_group_item_2021_SSS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_modifier_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_modifier_group_item",
                table: "tbl_modifier_group_item");

            migrationBuilder.AlterColumn<int>(
                name: "modifier_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "modifier_group_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_modifier_group_item",
                table: "tbl_modifier_group_item",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_item_modifier_group_id",
                table: "tbl_modifier_group_item",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_item_parent_id",
                table: "tbl_modifier_group_item",
                column: "parent_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_item_parent_id",
                table: "tbl_modifier_group_item",
                column: "parent_id",
                principalTable: "tbl_modifier_group_item",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier_group_item",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_modifier_id",
                table: "tbl_modifier_group_item",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_item_parent_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_modifier_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_modifier_group_item",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_modifier_group_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_parent_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "tbl_modifier_group_item");

            migrationBuilder.AlterColumn<int>(
                name: "modifier_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "modifier_group_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_modifier_group_item",
                table: "tbl_modifier_group_item",
                columns: new[] { "modifier_group_id", "modifier_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier_group_item",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_modifier_id",
                table: "tbl_modifier_group_item",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
