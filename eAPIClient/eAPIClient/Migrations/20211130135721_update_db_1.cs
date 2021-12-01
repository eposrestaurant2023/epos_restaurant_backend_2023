using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class update_db_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_working_day_working_day_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_cashier_shift_cashier_shift_id",
                table: "tbl_sale");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_working_day_working_day_id",
                table: "tbl_sale");

            migrationBuilder.AlterColumn<Guid>(
                name: "working_day_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "cashier_shift_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "working_day_id",
                table: "tbl_cashier_shift",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_working_day_working_day_id",
                table: "tbl_cashier_shift",
                column: "working_day_id",
                principalTable: "tbl_working_day",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_cashier_shift_cashier_shift_id",
                table: "tbl_sale",
                column: "cashier_shift_id",
                principalTable: "tbl_cashier_shift",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_working_day_working_day_id",
                table: "tbl_sale",
                column: "working_day_id",
                principalTable: "tbl_working_day",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cashier_shift_tbl_working_day_working_day_id",
                table: "tbl_cashier_shift");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_cashier_shift_cashier_shift_id",
                table: "tbl_sale");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_working_day_working_day_id",
                table: "tbl_sale");

            migrationBuilder.AlterColumn<Guid>(
                name: "working_day_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "cashier_shift_id",
                table: "tbl_sale",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "working_day_id",
                table: "tbl_cashier_shift",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cashier_shift_tbl_working_day_working_day_id",
                table: "tbl_cashier_shift",
                column: "working_day_id",
                principalTable: "tbl_working_day",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_cashier_shift_cashier_shift_id",
                table: "tbl_sale",
                column: "cashier_shift_id",
                principalTable: "tbl_cashier_shift",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_tbl_working_day_working_day_id",
                table: "tbl_sale",
                column: "working_day_id",
                principalTable: "tbl_working_day",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
