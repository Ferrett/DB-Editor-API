using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeveloperUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Developer_User_UserID",
                table: "Developer");

            migrationBuilder.DropIndex(
                name: "IX_Developer_UserID",
                table: "Developer");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Developer");

            migrationBuilder.CreateTable(
                name: "UserGame",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    GameID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGame", x => new { x.UserID, x.GameID });
                    table.ForeignKey(
                        name: "FK_UserGame_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGame_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGame_GameID",
                table: "UserGame",
                column: "GameID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGame");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Developer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Developer_UserID",
                table: "Developer",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Developer_User_UserID",
                table: "Developer",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
