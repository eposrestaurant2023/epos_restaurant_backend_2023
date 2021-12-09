using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class update_customer_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "business_branch_id",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "customer_group_name",
                table: "tbl_customer");
        }
    }
}
