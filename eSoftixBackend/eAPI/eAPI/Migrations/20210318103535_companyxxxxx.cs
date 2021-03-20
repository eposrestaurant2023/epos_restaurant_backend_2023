using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class companyxxxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_contact_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_contact");

            migrationBuilder.DropTable(
                name: "tbl_stock_location");

            migrationBuilder.DropIndex(
                name: "IX_tbl_contact_BusinessBranchModelid",
                table: "tbl_contact");

            migrationBuilder.DropColumn(
                name: "BusinessBranchModelid",
                table: "tbl_contact");

            migrationBuilder.AlterColumn<string>(
                name: "company_name_kh",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.CreateTable(
                name: "tbl_contact_related",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contact_id = table.Column<int>(type: "int", nullable: false),
                    project_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusinessBranchModelid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerModelid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectModelid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_contact_related", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_contact_related_tbl_business_branch_BusinessBranchModelid",
                        column: x => x.BusinessBranchModelid,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_contact_related_tbl_contact_contact_id",
                        column: x => x.contact_id,
                        principalTable: "tbl_contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_contact_related_tbl_customer_CustomerModelid",
                        column: x => x.CustomerModelid,
                        principalTable: "tbl_customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_contact_related_tbl_project_ProjectModelid",
                        column: x => x.ProjectModelid,
                        principalTable: "tbl_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_contact_related_BusinessBranchModelid",
                table: "tbl_contact_related",
                column: "BusinessBranchModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_contact_related_contact_id",
                table: "tbl_contact_related",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_contact_related_CustomerModelid",
                table: "tbl_contact_related",
                column: "CustomerModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_contact_related_ProjectModelid",
                table: "tbl_contact_related",
                column: "ProjectModelid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_contact_related");

            migrationBuilder.AlterColumn<string>(
                name: "company_name_kh",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Khmer_100_BIN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessBranchModelid",
                table: "tbl_contact",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_stock_location",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_default = table.Column<bool>(type: "bit", nullable: false),
                    stock_location_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_stock_location", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_stock_location_tbl_business_branch_business_branch_id",
                        column: x => x.business_branch_id,
                        principalTable: "tbl_business_branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_contact_BusinessBranchModelid",
                table: "tbl_contact",
                column: "BusinessBranchModelid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_stock_location_business_branch_id",
                table: "tbl_stock_location",
                column: "business_branch_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_contact_tbl_business_branch_BusinessBranchModelid",
                table: "tbl_contact",
                column: "BusinessBranchModelid",
                principalTable: "tbl_business_branch",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
