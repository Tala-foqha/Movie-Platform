using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviePlatform1.DAL.Migrations
{
    /// <inheritdoc />
    public partial class editting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTranslation_Categories_CategoryId",
                table: "CategoryTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieTranslation_Movies_MovieId",
                table: "MovieTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieTranslation",
                table: "MovieTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryTranslation",
                table: "CategoryTranslation");

            migrationBuilder.RenameTable(
                name: "MovieTranslation",
                newName: "movieTranslations");

            migrationBuilder.RenameTable(
                name: "CategoryTranslation",
                newName: "CategoryTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_MovieTranslation_MovieId",
                table: "movieTranslations",
                newName: "IX_movieTranslations_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryTranslation_CategoryId",
                table: "CategoryTranslations",
                newName: "IX_CategoryTranslations_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movieTranslations",
                table: "movieTranslations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryTranslations",
                table: "CategoryTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTranslations_Categories_CategoryId",
                table: "CategoryTranslations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movieTranslations_Movies_MovieId",
                table: "movieTranslations",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTranslations_Categories_CategoryId",
                table: "CategoryTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_movieTranslations_Movies_MovieId",
                table: "movieTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movieTranslations",
                table: "movieTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryTranslations",
                table: "CategoryTranslations");

            migrationBuilder.RenameTable(
                name: "movieTranslations",
                newName: "MovieTranslation");

            migrationBuilder.RenameTable(
                name: "CategoryTranslations",
                newName: "CategoryTranslation");

            migrationBuilder.RenameIndex(
                name: "IX_movieTranslations_MovieId",
                table: "MovieTranslation",
                newName: "IX_MovieTranslation_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryTranslations_CategoryId",
                table: "CategoryTranslation",
                newName: "IX_CategoryTranslation_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieTranslation",
                table: "MovieTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryTranslation",
                table: "CategoryTranslation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTranslation_Categories_CategoryId",
                table: "CategoryTranslation",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieTranslation_Movies_MovieId",
                table: "MovieTranslation",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
