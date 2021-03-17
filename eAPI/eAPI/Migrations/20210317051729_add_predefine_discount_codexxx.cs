using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_predefine_discount_codexxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_purchase_order_payment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_purchase_order_payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_create_payment_in_puchase_order = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    payment_amount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    payment_date = table.Column<DateTime>(type: "date", nullable: false),
                    payment_note = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    payment_type_id = table.Column<int>(type: "int", nullable: false),
                    purchase_order_id = table.Column<int>(type: "int", nullable: false),
                    reference_number = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_purchase_order_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_payment_tbl_payment_type_payment_type_id",
                        column: x => x.payment_type_id,
                        principalTable: "tbl_payment_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_purchase_order_payment_tbl_purchase_order_purchase_order_id",
                        column: x => x.purchase_order_id,
                        principalTable: "tbl_purchase_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_payment_payment_type_id",
                table: "tbl_purchase_order_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_purchase_order_payment_purchase_order_id",
                table: "tbl_purchase_order_payment",
                column: "purchase_order_id");
        }
    }
}
