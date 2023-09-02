using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class removeFKGameToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Events_EventId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_EventId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Games_EventId",
                table: "Games",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Events_EventId",
                table: "Games",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
