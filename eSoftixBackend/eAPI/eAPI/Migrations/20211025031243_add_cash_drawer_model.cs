using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_cash_drawer_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_user",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_user",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "cash_drawer_id",
                table: "tbl_station",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_station",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_station",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_role",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_role",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_request_license",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_request_license",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_project_type",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_project_type",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_project",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_project",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_payment_type",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_payment_type",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_outlet",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_outlet",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_history",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_history",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_customer_group",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_customer_group",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_customer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_contact_related",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_contact_related",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_contact",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_contact",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_business_branch",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_business_branch",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "tbl_attach_files",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "tbl_attach_files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "last_modified_by",
                table: "Notes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "Khmer_100_BIN");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "tbl_cash_drawer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cash_drawer_name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Khmer_100_BIN"),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    last_modified_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, collation: "Khmer_100_BIN"),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cash_drawer", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_station_cash_drawer_id",
                table: "tbl_station",
                column: "cash_drawer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_station_tbl_cash_drawer_cash_drawer_id",
                table: "tbl_station",
                column: "cash_drawer_id",
                principalTable: "tbl_cash_drawer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_station_tbl_cash_drawer_cash_drawer_id",
                table: "tbl_station");

            migrationBuilder.DropTable(
                name: "tbl_cash_drawer");

            migrationBuilder.DropIndex(
                name: "IX_tbl_station_cash_drawer_id",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_user");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_user");

            migrationBuilder.DropColumn(
                name: "cash_drawer_id",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_role");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_role");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_request_license");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_request_license");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_project_type");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_project_type");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_outlet");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_outlet");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_contact_related");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_contact_related");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_contact");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_contact");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_business_branch");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_business_branch");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "last_modified_by",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "Notes");
        }
    }
}
