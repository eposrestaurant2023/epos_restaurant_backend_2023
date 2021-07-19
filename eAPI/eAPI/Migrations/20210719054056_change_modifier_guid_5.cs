using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_modifier_guid_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {




            migrationBuilder.AddColumn<Guid>(
                name: "product_modifier_id",
                table: "tbl_sale_product_modifier",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_id",
                table: "tbl_modifier_ingredient",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "modifer_group_id",
                table: "tbl_modifier_group_product_category",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_group_id",
                table: "tbl_modifier",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_group_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_group_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_modifier_ingredient",
                table: "tbl_modifier_ingredient",
                columns: new[] { "modifier_id", "ingredient_id" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_group_product_category_modifer_group_id",
                table: "tbl_modifier_group_product_category",
                column: "modifer_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_modifier_modifier_group_id",
                table: "tbl_modifier",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_modifier_group_id",
                table: "tbl_history",
                column: "modifier_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_modifier_group_id",
                table: "tbl_attach_files",
                column: "modifier_group_id");

                
             
 


        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
             
        }
    }
}
