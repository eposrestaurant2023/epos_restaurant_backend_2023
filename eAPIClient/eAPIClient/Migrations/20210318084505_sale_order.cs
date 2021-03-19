using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class sale_order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "sale_order_id",
                table: "tbl_sale_product",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "tbl_sale_order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale_order", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_sale_order_id",
                table: "tbl_sale_product",
                column: "sale_order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_tbl_sale_order_sale_order_id",
                table: "tbl_sale_product",
                column: "sale_order_id",
                principalTable: "tbl_sale_order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_tbl_sale_order_sale_order_id",
                table: "tbl_sale_product");

            migrationBuilder.DropTable(
                name: "tbl_sale_order");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_product_sale_order_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "sale_order_id",
                table: "tbl_sale_product");
        }
    }
}
