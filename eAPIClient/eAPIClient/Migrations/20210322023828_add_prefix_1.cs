using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class add_prefix_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PrefixPrice",
                table: "PrefixPrice");

            migrationBuilder.RenameTable(
                name: "PrefixPrice",
                newName: "tbl_prefix_price");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_prefix_price",
                table: "tbl_prefix_price",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_prefix_price",
                table: "tbl_prefix_price");

            migrationBuilder.RenameTable(
                name: "tbl_prefix_price",
                newName: "PrefixPrice");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrefixPrice",
                table: "PrefixPrice",
                column: "id");
        }
    }
}
