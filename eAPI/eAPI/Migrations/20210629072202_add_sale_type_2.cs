using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_sale_type_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_type_tbl_business_branch_business_branch_id",
                table: "tbl_sale_type");

            migrationBuilder.DropColumn(
                name: "outlet_id",
                table: "tbl_sale_type");

            migrationBuilder.AlterColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_sale_type",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sale_type_name",
                table: "tbl_sale_type",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_type_tbl_business_branch_business_branch_id",
                table: "tbl_sale_type",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_type_tbl_business_branch_business_branch_id",
                table: "tbl_sale_type");

            migrationBuilder.DropColumn(
                name: "sale_type_name",
                table: "tbl_sale_type");

            migrationBuilder.AlterColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_sale_type",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "outlet_id",
                table: "tbl_sale_type",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_type_tbl_business_branch_business_branch_id",
                table: "tbl_sale_type",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
