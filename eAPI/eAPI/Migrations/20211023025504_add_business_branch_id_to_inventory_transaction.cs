using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class add_business_branch_id_to_inventory_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "business_branch_id",
                table: "tbl_inventory_transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_inventory_transaction");
        }
    }
}
