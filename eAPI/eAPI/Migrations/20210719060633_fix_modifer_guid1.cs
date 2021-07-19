using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_modifer_guid1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
  
 

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_modifier_group_id",
                table: "tbl_history");

          

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_modifier_group_id",
                table: "tbl_attach_files");
 

            migrationBuilder.DropColumn(
                name: "sale_product_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "sale_product_modifier_id",
                table: "tbl_inventory_transaction");

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_history");

           

            migrationBuilder.DropColumn(
                name: "modifier_group_id",
                table: "tbl_attach_files");

        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "sale_product_id",
                table: "tbl_inventory_transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "sale_product_modifier_id",
                table: "tbl_inventory_transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_group_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_group_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "modifier_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

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
        }
    }
}
