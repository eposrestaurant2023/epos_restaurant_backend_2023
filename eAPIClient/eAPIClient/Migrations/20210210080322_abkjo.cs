using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class abkjo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer");

            migrationBuilder.DropTable(
                name: "tbl_customer_group");

            migrationBuilder.DropIndex(
                name: "IX_tbl_customer_customer_group_id",
                table: "tbl_customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_customer_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_group_name_en = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    customer_group_name_kh = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_customer_group", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_customer_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_customer_tbl_customer_group_customer_group_id",
                table: "tbl_customer",
                column: "customer_group_id",
                principalTable: "tbl_customer_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
