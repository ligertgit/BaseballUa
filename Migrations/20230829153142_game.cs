using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class game : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sport",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RunsVisitor = table.Column<int>(type: "int", nullable: true),
                    RunsHome = table.Column<int>(type: "int", nullable: true),
                    PlacedAt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HalfinningsPlayed = table.Column<int>(type: "int", nullable: true),
                    GameStatus = table.Column<int>(type: "int", nullable: false),
                    PointsVisitor = table.Column<int>(type: "int", nullable: true),
                    PointsHome = table.Column<int>(type: "int", nullable: true),
                    GameType = table.Column<int>(type: "int", nullable: false),
                    Tour = table.Column<int>(type: "int", nullable: true),
                    ConditionVisitor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConditionHome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_EventId",
                table: "Games",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "Sport",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 3);
        }
    }
}
