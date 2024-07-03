using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gourmet.Core.Migrations
{
    public partial class TEST : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ban",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ban",
                table: "AspNetUsers");
        }
    }
}
