using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class test_guid_insertxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_attach_files_tbl_customer_customer_id",
                table: "tbl_attach_files");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_customer_customer_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_history_customer_id",
                table: "tbl_history");

            migrationBuilder.DropIndex(
                name: "IX_tbl_customer_customer_group_id",
                table: "tbl_customer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_attach_files_customer_id",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "customer_group_id",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "tbl_attach_files");

            migrationBuilder.RenameColumn(
                name: "image_name",
                table: "tbl_user",
                newName: "photo");

            migrationBuilder.RenameColumn(
                name: "image_name",
                table: "tbl_table_group",
                newName: "photo");

            migrationBuilder.RenameColumn(
                name: "product_image",
                table: "tbl_product",
                newName: "photo");

            migrationBuilder.RenameColumn(
                name: "image_name",
                table: "tbl_payment_type",
                newName: "photo");

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "tbl_permission_option_role",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_build_in",
                table: "tbl_payment_type",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "tbl_payment_type",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "tbl_payment_type",
                type: "int",
                nullable: false,
                defaultValue: 0);

           

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "tbl_customer_group",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "tbl_permission_option_role");

            migrationBuilder.DropColumn(
                name: "is_build_in",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "note",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "note",
                table: "tbl_customer_group");

            migrationBuilder.RenameColumn(
                name: "photo",
                table: "tbl_user",
                newName: "image_name");

            migrationBuilder.RenameColumn(
                name: "photo",
                table: "tbl_table_group",
                newName: "image_name");

            migrationBuilder.RenameColumn(
                name: "photo",
                table: "tbl_product",
                newName: "product_image");

            migrationBuilder.RenameColumn(
                name: "photo",
                table: "tbl_payment_type",
                newName: "image_name");

            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                table: "tbl_history",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "tbl_customer_group",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "customer_group_id",
                table: "tbl_customer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                table: "tbl_attach_files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_history_customer_id",
                table: "tbl_history",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_customer_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attach_files_customer_id",
                table: "tbl_attach_files",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_attach_files_tbl_customer_customer_id",
                table: "tbl_attach_files",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id",
                principalTable: "tbl_customer_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_customer_customer_id",
                table: "tbl_history",
                column: "customer_id",
                principalTable: "tbl_customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
