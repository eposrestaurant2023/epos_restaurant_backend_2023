using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_invtoryxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_product_type_product_type_id",
                table: "tbl_product");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_purchase_order_product_tbl_product_type_product_type_id",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_product_tbl_product_type_product_type_id",
                table: "tbl_sale_product");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_take_product_tbl_product_type_product_type_id",
                table: "tbl_stock_take_product");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_stock_transfer_product_tbl_product_type_product_type_id",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropTable(
                name: "tbl_product_type");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_transfer_product_product_type_id",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_stock_take_product_product_type_id",
                table: "tbl_stock_take_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_product_product_type_id",
                table: "tbl_sale_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_purchase_order_product_product_type_id",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_product_type_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_type_id",
                table: "tbl_stock_transfer_product");

            migrationBuilder.DropColumn(
                name: "product_type_id",
                table: "tbl_stock_take_product");

            migrationBuilder.DropColumn(
                name: "product_type_id",
                table: "tbl_sale_product");

            migrationBuilder.DropColumn(
                name: "product_type_id",
                table: "tbl_purchase_order_product");

            migrationBuilder.DropColumn(
                name: "product_type_id",
                table: "tbl_product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_type_id",
                table: "tbl_stock_transfer_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "product_type_id",
                table: "tbl_stock_take_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "product_type_id",
                table: "tbl_sale_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "product_type_id",
                table: "tbl_purchase_order_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "product_type_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbl_product_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    product_type_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_type", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_transfer_product_product_type_id",
                table: "tbl_stock_transfer_product",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_take_product_product_type_id",
                table: "tbl_stock_take_product",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_product_product_type_id",
                table: "tbl_sale_product",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_product_product_type_id",
                table: "tbl_purchase_order_product",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_product_type_id",
                table: "tbl_product",
                column: "product_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_product_type_product_type_id",
                table: "tbl_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_purchase_order_product_tbl_product_type_product_type_id",
                table: "tbl_purchase_order_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_sale_product_tbl_product_type_product_type_id",
                table: "tbl_sale_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_take_product_tbl_product_type_product_type_id",
                table: "tbl_stock_take_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_stock_transfer_product_tbl_product_type_product_type_id",
                table: "tbl_stock_transfer_product",
                column: "product_type_id",
                principalTable: "tbl_product_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
