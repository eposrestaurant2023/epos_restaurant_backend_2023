using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class wrong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_type_CurrencyShareModel_currency_id",
                table: "tbl_payment_type");

            migrationBuilder.DropTable(
                name: "CurrencyShareModel");

            migrationBuilder.CreateTable(
                name: "tbl_currency",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    currency_name_en = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_name_kh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    currency_format = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "Khmer_100_BIN"),
                    exchange_rate = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    change_exchange_rate = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    is_main = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_currency", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_type_tbl_currency_currency_id",
                table: "tbl_payment_type",
                column: "currency_id",
                principalTable: "tbl_currency",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_payment_type_tbl_currency_currency_id",
                table: "tbl_payment_type");

            migrationBuilder.DropTable(
                name: "tbl_currency");

            migrationBuilder.CreateTable(
                name: "CurrencyShareModel",
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
                    table.PrimaryKey("PK_CurrencyShareModel", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_payment_type_CurrencyShareModel_currency_id",
                table: "tbl_payment_type",
                column: "currency_id",
                principalTable: "CurrencyShareModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
