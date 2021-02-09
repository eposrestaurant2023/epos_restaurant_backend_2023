using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_price_rulexxxxxxxxxsx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "province_id",
                table: "tbl_vendor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    province_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_vendor_province_id",
                table: "tbl_vendor",
                column: "province_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_vendor_Provinces_province_id",
                table: "tbl_vendor",
                column: "province_id",
                principalTable: "Provinces",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_vendor_Provinces_province_id",
                table: "tbl_vendor");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_tbl_vendor_province_id",
                table: "tbl_vendor");

            migrationBuilder.DropColumn(
                name: "province_id",
                table: "tbl_vendor");
        }
    }
}
