using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class title_photo_nocascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsTitlePhotos_News_NewsId",
                table: "NewsTitlePhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsTitlePhotos_Photos_PhotoId",
                table: "NewsTitlePhotos");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTitlePhotos_News_NewsId",
                table: "NewsTitlePhotos",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTitlePhotos_Photos_PhotoId",
                table: "NewsTitlePhotos",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsTitlePhotos_News_NewsId",
                table: "NewsTitlePhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsTitlePhotos_Photos_PhotoId",
                table: "NewsTitlePhotos");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTitlePhotos_News_NewsId",
                table: "NewsTitlePhotos",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTitlePhotos_Photos_PhotoId",
                table: "NewsTitlePhotos",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
