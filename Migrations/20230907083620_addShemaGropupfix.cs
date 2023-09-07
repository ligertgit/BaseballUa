using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class addShemaGropupfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchemaGroupId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Games_SchemaGroupId",
                table: "Games",
                column: "SchemaGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_SchemaGroups_SchemaGroupId",
                table: "Games",
                column: "SchemaGroupId",
                principalTable: "SchemaGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_SchemaGroups_SchemaGroupId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_SchemaGroupId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "SchemaGroupId",
                table: "Games");
        }
    }
}
