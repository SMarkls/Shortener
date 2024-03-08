using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkShortener.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedRedis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "ExpirationTime",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Used",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<string>(
                name: "RefreshTokenToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RefreshTokenToken",
                table: "AspNetUsers",
                column: "RefreshTokenToken");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RefreshTokens_RefreshTokenToken",
                table: "AspNetUsers",
                column: "RefreshTokenToken",
                principalTable: "RefreshTokens",
                principalColumn: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RefreshTokens_RefreshTokenToken",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RefreshTokenToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenToken",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationTime",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Used",
                table: "RefreshTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RefreshTokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
