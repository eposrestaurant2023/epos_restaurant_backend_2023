using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class ddd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sale_id",
                table: "tbl_inventory_transaction");
                        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sale_id",
                table: "tbl_inventory_transaction",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "customer_name_kh",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "customer_name_en",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldCollation: "Khmer_100_BIN");
        }
    }
}
