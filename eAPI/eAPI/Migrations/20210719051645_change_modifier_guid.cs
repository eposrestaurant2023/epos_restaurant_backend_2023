using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_modifier_guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_group_modifier_group_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_modifier_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_modifier_group_modifier_group_id",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_modifier_modifier_id",
                table: "tbl_history");

            
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_group_ModifierGroupModelid",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_modifier_group_ModifierGroupModelid",
                table: "tbl_history");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_item_ModifierGroupItemModelid",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_group_ModifierGroupModelid",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_item_tbl_modifier_ModifierModelid",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_group_product_category_tbl_modifier_group_ModifierGroupModelid",
                table: "tbl_modifier_group_product_category");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_modifier_ingredient_tbl_modifier_ModifierModelid",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_ModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_ModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_modifier_ProductModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_modifier_ingredient",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_ingredient_ModifierModelid",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_product_category_ModifierGroupModelid",
                table: "tbl_modifier_group_product_category");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_ModifierGroupItemModelid",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_ModifierGroupModelid",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_modifier_group_item_ModifierModelid",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_ModifierGroupModelid",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_ModifierGroupModelid",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "ModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "ProductModifierModelid",
                table: "tbl_product_modifier");

            migrationBuilder.DropColumn(
                name: "ModifierModelid",
                table: "tbl_modifier_ingredient");

            migrationBuilder.DropColumn(
                name: "ModifierGroupModelid",
                table: "tbl_modifier_group_product_category");

            migrationBuilder.DropColumn(
                name: "ModifierGroupItemModelid",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "ModifierGroupModelid",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "ModifierModelid",
                table: "tbl_modifier_group_item");

            migrationBuilder.DropColumn(
                name: "ModifierGroupModelid",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "ModifierGroupModelid",
                table: "tbl_attach_files");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_modifier_ingredient",
                newName: "modifier_id");

            migrationBuilder.AddColumn<int>(
                name: "product_modifier_id",
                table: "tbl_sale_product_modifier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "modifier_group_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "modifier_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "modifier_id",
                table: "tbl_modifier_ingredient",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "modifer_group_id",
                table: "tbl_modifier_group_product_category",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "modifier_group_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "modifier_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "tbl_modifier_group_item",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "tbl_modifier_group",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "tbl_modifier",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "modifier_group_id",
                table: "tbl_modifier",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "modifier_group_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "modifier_id",
                table: "tbl_history",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "modifier_group_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "modifier_id",
                table: "tbl_attach_files",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_modifier_ingredient",
                table: "tbl_modifier_ingredient",
                columns: new[] { "modifier_id", "ingredient_id" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_modifier_id",
                table: "tbl_product_modifier",
                column: "modifier_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_product_category_modifer_group_id",
                table: "tbl_modifier_group_product_category",
                column: "modifer_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_item_modifier_group_id",
                table: "tbl_modifier_group_item",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_item_modifier_id",
                table: "tbl_modifier_group_item",
                column: "modifier_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_item_parent_id",
                table: "tbl_modifier_group_item",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_modifier_group_id",
                table: "tbl_modifier",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_modifier_group_id",
                table: "tbl_history",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_modifier_id",
                table: "tbl_history",
                column: "modifier_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_modifier_group_id",
                table: "tbl_attach_files",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_modifier_id",
                table: "tbl_attach_files",
                column: "modifier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_group_modifier_group_id",
                table: "tbl_attach_files",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_modifier_modifier_id",
                table: "tbl_attach_files",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_modifier_group_modifier_group_id",
                table: "tbl_history",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_modifier_modifier_id",
                table: "tbl_history",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_tbl_modifier_group_modifier_group_id",
                table: "tbl_modifier",
                column: "modifier_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_group_product_category_tbl_modifier_group_modifer_group_id",
                table: "tbl_modifier_group_product_category",
                column: "modifer_group_id",
                principalTable: "tbl_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_modifier_ingredient_tbl_modifier_modifier_id",
                table: "tbl_modifier_ingredient",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_modifier_modifier_id",
                table: "tbl_product_modifier",
                column: "modifier_id",
                principalTable: "tbl_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_modifier_tbl_product_modifier_parent_id",
                table: "tbl_product_modifier",
                column: "parent_id",
                principalTable: "tbl_product_modifier",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
