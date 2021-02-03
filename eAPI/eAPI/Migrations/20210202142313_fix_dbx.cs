using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_dbx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_price_tbl_product_product_id",
                table: "tbl_product_price");

            migrationBuilder.DropColumn(
                name: "multiplier",
                table: "tbl_product_price");

            migrationBuilder.DropColumn(
                name: "portion_name",
                table: "tbl_product_price");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "tbl_product_price",
                newName: "product_portion_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_product_price_product_id",
                table: "tbl_product_price",
                newName: "IX_tbl_product_price_product_portion_id");

            migrationBuilder.CreateTable(
                name: "tbl_product_portion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    portion_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "Khmer_100_BIN"),
                    multiplier = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_portion", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_price_tbl_product_portion_product_portion_id",
                table: "tbl_product_price",
                column: "product_portion_id",
                principalTable: "tbl_product_portion",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_price_tbl_product_portion_product_portion_id",
                table: "tbl_product_price");

            migrationBuilder.DropTable(
                name: "tbl_product_portion");

            migrationBuilder.RenameColumn(
                name: "product_portion_id",
                table: "tbl_product_price",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_product_price_product_portion_id",
                table: "tbl_product_price",
                newName: "IX_tbl_product_price_product_id");

            migrationBuilder.AddColumn<decimal>(
                name: "multiplier",
                table: "tbl_product_price",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "portion_name",
                table: "tbl_product_price",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                collation: "Khmer_100_BIN");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_price_tbl_product_product_id",
                table: "tbl_product_price",
                column: "product_id",
                principalTable: "tbl_product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
