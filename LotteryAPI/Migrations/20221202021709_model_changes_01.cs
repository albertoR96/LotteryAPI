using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotteryAPI.Migrations
{
    public partial class model_changes_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_Participants_WinnerId",
                table: "Prizes");

            migrationBuilder.DropIndex(
                name: "IX_Prizes_WinnerId",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Prizes");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "End",
                table: "Games",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Begin",
                table: "Games",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameStatus",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Administrators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GamePrizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    PrizeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePrizes_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePrizes_Prizes_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParticipantId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GameNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantGames_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Winners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    ParticipantId = table.Column<int>(type: "int", nullable: false),
                    PrizeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Winners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Winners_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Winners_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Winners_Prizes_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePrizes_GameId",
                table: "GamePrizes",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePrizes_PrizeId",
                table: "GamePrizes",
                column: "PrizeId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantGames_GameId",
                table: "ParticipantGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantGames_ParticipantId",
                table: "ParticipantGames",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_GameId",
                table: "Winners",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_ParticipantId",
                table: "Winners",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_PrizeId",
                table: "Winners",
                column: "PrizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePrizes");

            migrationBuilder.DropTable(
                name: "ParticipantGames");

            migrationBuilder.DropTable(
                name: "Winners");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "GameStatus",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Administrators");

            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "Prizes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "End",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Begin",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_WinnerId",
                table: "Prizes",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prizes_Participants_WinnerId",
                table: "Prizes",
                column: "WinnerId",
                principalTable: "Participants",
                principalColumn: "Id");
        }
    }
}
