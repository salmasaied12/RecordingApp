using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordingApp.Migrations
{
    /// <inheritdoc />
    public partial class Transcription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Transcription",
                table: "AudioFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TranscriptionText",
                table: "AudioFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Transcription",
                table: "AudioFiles");

            migrationBuilder.DropColumn(
                name: "TranscriptionText",
                table: "AudioFiles");
        }
    }
}
