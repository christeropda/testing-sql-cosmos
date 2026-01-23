using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cosmosdb_test.Migrations
{
    /// <inheritdoc />
    public partial class RenamedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayedGame");

            migrationBuilder.CreateTable(
                name: "CompletedGameStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerMove = table.Column<int>(type: "int", nullable: false),
                    MachineMove = table.Column<int>(type: "int", nullable: false),
                    MatchResult = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedGameStates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedGameStates");

            migrationBuilder.CreateTable(
                name: "PlayedGame",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MachineMove = table.Column<int>(type: "int", nullable: false),
                    MatchResult = table.Column<int>(type: "int", nullable: false),
                    PlayerMove = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayedGame", x => x.Id);
                });
        }
    }
}
