using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class addShemaGropup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_EventSchemaItems_EventSchemaItemId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_EventSchemaItemId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "EventSchemaItemId",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "SchemaGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    EventSchemaItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemaGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchemaGroups_EventSchemaItems_EventSchemaItemId",
                        column: x => x.EventSchemaItemId,
                        principalTable: "EventSchemaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchemaGroups_EventSchemaItemId",
                table: "SchemaGroups",
                column: "EventSchemaItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchemaGroups");

            migrationBuilder.AddColumn<int>(
                name: "EventSchemaItemId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
