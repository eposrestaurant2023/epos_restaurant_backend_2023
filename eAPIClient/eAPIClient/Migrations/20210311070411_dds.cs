using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class dds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_tbl_payment_type_payment_type_id",
                table: "tbl_payment");

            migrationBuilder.DropTable(
                name: "tbl_payment_type");

            migrationBuilder.DropTable(
                name: "tbl_product_type");

            migrationBuilder.DropTable(
                name: "tbl_currency");

            migrationBuilder.DropIndex(
                name: "IX_tbl_payment_payment_type_id",
                table: "tbl_payment");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_cashier_shift_id",
                table: "tbl_sale",
                column: "cashier_shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_working_day_id",
                table: "tbl_sale",
                column: "working_day_id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_cashier_shift_cashier_shift_id",
                table: "tbl_sale");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_sale_tbl_working_day_working_day_id",
                table: "tbl_sale");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_cashier_shift_id",
                table: "tbl_sale");

            migrationBuilder.DropIndex(
                name: "IX_tbl_sale_working_day_id",
                table: "tbl_sale");

            migrationBuilder.CreateTable(
                name: "tbl_currency",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    change_exchange_rate = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    currency_format = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    product_type_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payment_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    currency_id = table.Column<int>(type: "int", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_payment_type_tbl_currency_currency_id",
                        column: x => x.currency_id,
                        principalTable: "tbl_currency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_payment_type_id",
                table: "tbl_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_type_currency_id",
                table: "tbl_payment_type",
                column: "currency_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_tbl_payment_type_payment_type_id",
                table: "tbl_payment",
                column: "payment_type_id",
                principalTable: "tbl_payment_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
