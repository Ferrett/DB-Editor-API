using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class NamingUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AchievementsGot",
                table: "GameStats",
                newName: "AchievementsGotten");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "User",
                type: "character varying(99)",
                maxLength: 99,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Game",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Developer",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AchievementsGotten",
                table: "GameStats",
                newName: "AchievementsGot");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "User",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(99)",
                oldMaxLength: 99);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Game",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Developer",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);
        }
    }
}
