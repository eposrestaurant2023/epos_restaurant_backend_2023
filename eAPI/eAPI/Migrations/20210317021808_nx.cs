using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class nx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_payment_payment_id",
                table: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_payment");

            migrationBuilder.RenameColumn(
                name: "payment_id",
                table: "tbl_history",
                newName: "sale_payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_history_payment_id",
                table: "tbl_history",
                newName: "IX_tbl_history_sale_payment_id");

            migrationBuilder.RenameColumn(
                name: "discount_label",
                table: "tbl_discount_code",
                newName: "discount_code");

            migrationBuilder.CreateTable(
                name: "tbl_sale_payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    sale_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_date = table.Column<DateTime>(type: "date", nullable: false),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    payment_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    payment_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    is_create_payment_in_sale_order = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sale_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_sale_payment_tbl_payment_type_payment_type_id",
                        column: x => x.payment_type_id,
                        principalTable: "tbl_payment_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_sale_payment_tbl_sale_sale_id",
                        column: x => x.sale_id,
                        principalTable: "tbl_sale",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_payment_payment_type_id",
                table: "tbl_sale_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sale_payment_sale_id",
                table: "tbl_sale_payment",
                column: "sale_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_sale_payment_sale_payment_id",
                table: "tbl_history",
                column: "sale_payment_id",
                principalTable: "tbl_sale_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_history_tbl_sale_payment_sale_payment_id",
                table: "tbl_history");

            migrationBuilder.DropTable(
                name: "tbl_sale_payment");

            migrationBuilder.RenameColumn(
                name: "sale_payment_id",
                table: "tbl_history",
                newName: "payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_history_sale_payment_id",
                table: "tbl_history",
                newName: "IX_tbl_history_payment_id");

            migrationBuilder.RenameColumn(
                name: "discount_code",
                table: "tbl_discount_code",
                newName: "discount_label");

            migrationBuilder.CreateTable(
                name: "tbl_payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_create_payment_in_sale_order = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    outlet_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    payment_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    payment_date = table.Column<DateTime>(type: "date", nullable: false),
                    payment_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    sale_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_payment_tbl_payment_type_payment_type_id",
                        column: x => x.payment_type_id,
                        principalTable: "tbl_payment_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_payment_tbl_sale_sale_id",
                        column: x => x.sale_id,
                        principalTable: "tbl_sale",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_payment_type_id",
                table: "tbl_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payment_sale_id",
                table: "tbl_payment",
                column: "sale_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_history_tbl_payment_payment_id",
                table: "tbl_history",
                column: "payment_id",
                principalTable: "tbl_payment",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
