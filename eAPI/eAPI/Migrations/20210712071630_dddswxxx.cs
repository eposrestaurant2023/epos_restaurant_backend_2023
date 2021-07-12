using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class dddswxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency_id",
                table: "tbl_predefine_payment_amount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currency_id",
                table: "tbl_predefine_payment_amount",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
