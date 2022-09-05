using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class change_table_discount_promotion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_product_disocunt_promotion");

            migrationBuilder.AlterColumn<DateTime>(
                name: "start_date",
                table: "tbl_discount_promotion",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_date",
                table: "tbl_discount_promotion",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.CreateTable(
                name: "tbl_discount_promotion_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    dicount_promotion_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_category_id = table.Column<int>(type: "int", nullable: false),
                    discount_percentage = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    last_modified_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_discount_promotion_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_discount_promotion_item_tbl_discount_promotion_dicount_promotion_id",
                        column: x => x.dicount_promotion_id,
                        principalTable: "tbl_discount_promotion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_discount_promotion_item_tbl_product_category_product_category_id",
                        column: x => x.product_category_id,
                        principalTable: "tbl_product_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_discount_promotion_item_dicount_promotion_id",
                table: "tbl_discount_promotion_item",
                column: "dicount_promotion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_discount_promotion_item_product_category_id",
                table: "tbl_discount_promotion_item",
                column: "product_category_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_discount_promotion_item");

            migrationBuilder.AlterColumn<DateTime>(
                name: "start_date",
                table: "tbl_discount_promotion",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_date",
                table: "tbl_discount_promotion",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_product_disocunt_promotion",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    dicount_promotion_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    discount_percentage = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    last_modified_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    product_category_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product_disocunt_promotion", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_product_disocunt_promotion_tbl_discount_promotion_dicount_promotion_id",
                        column: x => x.dicount_promotion_id,
                        principalTable: "tbl_discount_promotion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_product_disocunt_promotion_tbl_product_category_product_category_id",
                        column: x => x.product_category_id,
                        principalTable: "tbl_product_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_disocunt_promotion_dicount_promotion_id",
                table: "tbl_product_disocunt_promotion",
                column: "dicount_promotion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_disocunt_promotion_product_category_id",
                table: "tbl_product_disocunt_promotion",
                column: "product_category_id");
        }
    }
}
