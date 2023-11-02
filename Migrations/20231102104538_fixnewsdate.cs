﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseballUa.Migrations
{
    /// <inheritdoc />
    public partial class fixnewsdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PDate",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "PDate",
                table: "Videos",
                newName: "PublishDate");

            migrationBuilder.RenameColumn(
                name: "PDate",
                table: "Albums",
                newName: "PublishDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "Videos",
                newName: "PDate");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "Albums",
                newName: "PDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "PDate",
                table: "News",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
