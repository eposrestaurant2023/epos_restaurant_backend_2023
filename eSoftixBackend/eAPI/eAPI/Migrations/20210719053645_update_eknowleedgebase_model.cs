using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_eknowleedgebase_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_user");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_station");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_role");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_request_license");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_project_type");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_payment_type");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_outlet");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_history");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_customer_group");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_contact_related");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_contact");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_business_branch");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "tbl_attach_files");

            migrationBuilder.DropColumn(
                name: "last_modified",
                table: "Notes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_user",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_station",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_role",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_request_license",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_project_type",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_project",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_payment_type",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_outlet",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_history",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_customer_group",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_contact_related",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_contact",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_business_branch",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "tbl_attach_files",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified",
                table: "Notes",
                type: "datetime2",
                nullable: true);
        }
    }
}
