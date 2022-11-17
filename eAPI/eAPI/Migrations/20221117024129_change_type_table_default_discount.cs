using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_type_table_default_discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "default_discount_percentage",
                table: "tbl_table",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "default_discount_percentage",
                table: "tbl_table",
                type: "decimal(19,8)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
