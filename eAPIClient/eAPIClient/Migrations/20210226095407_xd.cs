using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPIClient.Migrations
{
    public partial class xd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "allow_in_pos_order_list",
                table: "tbl_sale_product_status",
                newName: "allow_display_in_pos_order_list");

            migrationBuilder.AlterColumn<int>(
                name: "submited_status_id",
                table: "tbl_sale_product_status",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "tbl_sale_product_status",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "note",
                table: "tbl_sale_product_status");

            migrationBuilder.RenameColumn(
                name: "allow_display_in_pos_order_list",
                table: "tbl_sale_product_status",
                newName: "allow_in_pos_order_list");

            migrationBuilder.AlterColumn<bool>(
                name: "submited_status_id",
                table: "tbl_sale_product_status",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
