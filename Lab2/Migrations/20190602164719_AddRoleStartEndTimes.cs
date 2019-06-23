using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Lab2.Migrations
{
    public partial class AddRoleStartEndTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "UserUserRole",
                nullable: true);

            migrationBuilder.AddColumn<System.DateTime>(
              name: "StartTime",
              table: "UserUserRole",
              nullable: false,
              defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "EndTime",
               table: "UserUserRole");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "UserUserRole");
        }
    }

   
}
