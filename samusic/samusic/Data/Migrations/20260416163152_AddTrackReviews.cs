using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samusic.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpotifyTrackId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArtistNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlbumName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlbumImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpotifyUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackReviews", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackReviews");
        }
    }
}
