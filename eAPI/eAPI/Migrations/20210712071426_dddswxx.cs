using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class dddswxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_predefined_payment_amount");

            migrationBuilder.CreateTable(
                name: "tbl_predefine_payment_amount",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    predefine_value = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_predefine_payment_amount", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_default_stock_location_product_station_id",
                table: "tbl_default_stock_location_product",
                column: "station_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_default_stock_location_product_tbl_station_station_id",
                table: "tbl_default_stock_location_product",
                column: "station_id",
                principalTable: "tbl_station",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_default_stock_location_product_tbl_station_station_id",
                table: "tbl_default_stock_location_product");

            migrationBuilder.DropTable(
                name: "tbl_predefine_payment_amount");

            migrationBuilder.DropIndex(
                name: "IX_tbl_default_stock_location_product_station_id",
                table: "tbl_default_stock_location_product");

            migrationBuilder.CreateTable(
                name: "tbl_predefined_payment_amount",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    prefix_price_value = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_predefined_payment_amount", x => x.id);
                });
        }
    }
}
