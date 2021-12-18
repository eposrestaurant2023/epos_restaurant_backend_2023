using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_sale_type_to_tbl_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "user_code",
                table: "tbl_user",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "sale_type",
                table: "tbl_table",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "table_group_name",
                table: "tbl_sale",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sale_type",
                table: "tbl_table");

            migrationBuilder.DropColumn(
                name: "table_group_name",
                table: "tbl_sale");

            migrationBuilder.AlterColumn<string>(
                name: "user_code",
                table: "tbl_user",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Khmer_100_BIN");
        }
    }
}
