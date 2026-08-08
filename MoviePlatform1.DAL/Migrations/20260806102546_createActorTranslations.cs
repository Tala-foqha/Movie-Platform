using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviePlatform1.DAL.Migrations
{
    /// <inheritdoc />
    public partial class createActorTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Actors");

            migrationBuilder.AddColumn<int>(
                name: "ActorId",
                table: "movieTranslations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_movieTranslations_ActorId",
                table: "movieTranslations",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_movieTranslations_Actors_ActorId",
                table: "movieTranslations",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movieTranslations_Actors_ActorId",
                table: "movieTranslations");

            migrationBuilder.DropIndex(
                name: "IX_movieTranslations_ActorId",
                table: "movieTranslations");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "movieTranslations");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
