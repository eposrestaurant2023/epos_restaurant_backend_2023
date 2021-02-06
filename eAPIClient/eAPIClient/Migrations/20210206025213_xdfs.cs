using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class xdfs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_product_price",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_portion_id = table.Column<int>(type: "int", nullable: false),
                    prices = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_price", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_product_price");
        }
    }
}
