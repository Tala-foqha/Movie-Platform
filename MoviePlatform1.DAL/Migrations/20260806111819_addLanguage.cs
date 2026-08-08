using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviePlatform1.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "ActorTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "ActorTranslations");
        }
    }
}
