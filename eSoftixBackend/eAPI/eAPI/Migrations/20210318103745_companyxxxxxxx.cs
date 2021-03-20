using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class companyxxxxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_stock_location",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stock_location_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_default = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_stock_location", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_stock_location_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_business_branch_id",
                table: "tbl_stock_location",
                column: "business_branch_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_stock_location");
        }
    }
}
