using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class EventToTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventToTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventToTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventToTeams_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventToTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventToTeams_EventId",
                table: "EventToTeams",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventToTeams_TeamId",
                table: "EventToTeams",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventToTeams");
        }
    }
}
