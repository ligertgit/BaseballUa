using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class fixGameGameTypeToFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Events_EventId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "GameType",
                table: "Games",
                newName: "EventSchemaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_EventSchemaItemId",
                table: "Games",
                column: "EventSchemaItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_EventSchemaItems_EventSchemaItemId",
                table: "Games",
                column: "EventSchemaItemId",
                principalTable: "EventSchemaItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Events_EventId",
                table: "Games",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_EventSchemaItems_EventSchemaItemId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Events_EventId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_EventSchemaItemId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "EventSchemaItemId",
                table: "Games",
                newName: "GameType");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Events_EventId",
                table: "Games",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
