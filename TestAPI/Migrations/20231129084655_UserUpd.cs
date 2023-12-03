using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
