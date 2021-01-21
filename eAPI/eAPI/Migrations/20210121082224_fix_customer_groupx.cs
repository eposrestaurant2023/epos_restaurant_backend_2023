using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_customer_groupx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "my_id",
                table: "tbl_customer_group");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "my_id",
                table: "tbl_customer_group",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_customer_group",
                table: "tbl_customer_group",
                column: "my_id");
        }
    }
}
