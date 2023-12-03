using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class GameNamingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Game",
                newName: "PriceUsd");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Game",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Game",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PriceUsd",
                table: "Game",
                newName: "Price");
        }
    }
}
