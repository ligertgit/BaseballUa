using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class testnotmapped4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_HomeTeamId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_VisitorTeamId",
                table: "Games");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_HomeTeamId",
                table: "Games",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_VisitorTeamId",
                table: "Games",
                column: "VisitorTeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_HomeTeamId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_VisitorTeamId",
                table: "Games");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_HomeTeamId",
                table: "Games",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_VisitorTeamId",
                table: "Games",
                column: "VisitorTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
