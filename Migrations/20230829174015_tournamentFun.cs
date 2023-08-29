using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class tournamentFun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFun",
                table: "Tournaments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFun",
                table: "Tournaments");
        }
    }
}
