using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class ttxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_outlets_tbl_business_branch_business_branch_id",
                table: "tbl_outlets");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_station_tbl_outlets_outlet_id",
                table: "tbl_station");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_outlets",
                table: "tbl_outlets");

            migrationBuilder.RenameTable(
                name: "tbl_outlets",
                newName: "tbl_outlet");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_outlets_business_branch_id",
                table: "tbl_outlet",
                newName: "IX_tbl_outlet_business_branch_id");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "tbl_user",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<Guid>(
                name: "project_id",
                table: "tbl_business_branch",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_outlet",
                table: "tbl_outlet",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_business_branch_project_id",
                table: "tbl_business_branch",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_business_branch_tbl_project_project_id",
                table: "tbl_business_branch",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_outlet_tbl_business_branch_business_branch_id",
                table: "tbl_outlet",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_station_tbl_outlet_outlet_id",
                table: "tbl_station",
                column: "outlet_id",
                principalTable: "tbl_outlet",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_business_branch_tbl_project_project_id",
                table: "tbl_business_branch");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_outlet_tbl_business_branch_business_branch_id",
                table: "tbl_outlet");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_station_tbl_outlet_outlet_id",
                table: "tbl_station");

            migrationBuilder.DropIndex(
                name: "IX_tbl_business_branch_project_id",
                table: "tbl_business_branch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_outlet",
                table: "tbl_outlet");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "tbl_business_branch");

            migrationBuilder.RenameTable(
                name: "tbl_outlet",
                newName: "tbl_outlets");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_outlet_business_branch_id",
                table: "tbl_outlets",
                newName: "IX_tbl_outlets_business_branch_id");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "tbl_user",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_outlets",
                table: "tbl_outlets",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_outlets_tbl_business_branch_business_branch_id",
                table: "tbl_outlets",
                column: "business_branch_id",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_station_tbl_outlets_outlet_id",
                table: "tbl_station",
                column: "outlet_id",
                principalTable: "tbl_outlets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
