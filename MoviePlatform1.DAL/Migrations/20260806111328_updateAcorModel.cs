using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviePlatform1.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateAcorModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
