using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class new_product_typex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "background_color",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "cost",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "created_outlet_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "has_variant",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "is_auto_generate_product_code",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "keyword",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "photo",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "price",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_code_1",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_code_2",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "product_name",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "text_color",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "total_variants",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "track_quantity_on_variant",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "use_variant_1",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "variant_1_name",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "variant_2_name",
                table: "tbl_product");

            migrationBuilder.RenameColumn(
                name: "variant_3_name",
                table: "tbl_product",
                newName: "image_name");

            migrationBuilder.RenameColumn(
                name: "use_variant_price",
                table: "tbl_product",
                newName: "is_open_product");

            migrationBuilder.RenameColumn(
                name: "use_variant_3",
                table: "tbl_product",
                newName: "is_auto_generate_code");

            migrationBuilder.RenameColumn(
                name: "use_variant_2",
                table: "tbl_product",
                newName: "is_allow_free");

            migrationBuilder.AlterColumn<string>(
                name: "product_name_kh",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "product_code",
                table: "tbl_product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_name_en",
                table: "tbl_product",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                collation: "Khmer_100_BIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_name_en",
                table: "tbl_product");

            migrationBuilder.RenameColumn(
                name: "is_open_product",
                table: "tbl_product",
                newName: "use_variant_price");

            migrationBuilder.RenameColumn(
                name: "is_auto_generate_code",
                table: "tbl_product",
                newName: "use_variant_3");

            migrationBuilder.RenameColumn(
                name: "is_allow_free",
                table: "tbl_product",
                newName: "use_variant_2");

            migrationBuilder.RenameColumn(
                name: "image_name",
                table: "tbl_product",
                newName: "variant_3_name");

            migrationBuilder.AlterColumn<string>(
                name: "product_name_kh",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AlterColumn<string>(
                name: "product_code",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "background_color",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "tbl_product",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "created_outlet_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "has_variant",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_auto_generate_product_code",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "keyword",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "photo",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "tbl_product",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "product_code_1",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_code_2",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "product_name",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<decimal>(
                name: "quantity",
                table: "tbl_product",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "text_color",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<int>(
                name: "total_variants",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "track_quantity_on_variant",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "use_variant_1",
                table: "tbl_product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "variant_1_name",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<string>(
                name: "variant_2_name",
                table: "tbl_product",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN");
        }
    }
}
