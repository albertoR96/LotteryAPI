using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryAPI.Migrations
{
    public partial class model_changes_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Begin",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "Games",
                newName: "PlayedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayedAt",
                table: "Games",
                newName: "End");

            migrationBuilder.AddColumn<DateTime>(
                name: "Begin",
                table: "Games",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
