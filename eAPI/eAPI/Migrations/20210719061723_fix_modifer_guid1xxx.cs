using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eAPI.Migrations
{
    public partial class fix_modifer_guid1xxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
 

            migrationBuilder.AlterColumn<int>(
                name: "modifier_group_id",
                table: "tbl_product_modifier",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
