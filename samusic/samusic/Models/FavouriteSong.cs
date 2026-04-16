using System.ComponentModel.DataAnnotations;

namespace samusic.Models
{
    // Stores data for tracks saved to a user's favourites list
    public class FavouriteSong
    {
        // Unique ID for each saved favourite entry
        public int Id { get; set; }

        // Stores Spotify ID of the track
        [Required]
        public string SpotifyTrackId { get; set; } = string.Empty;

        // Stores name of the track
        [Required]
        public string TrackName { get; set; } = string.Empty;

        // Stores artist or artists linked to the track
        [Required]
        public string ArtistNames { get; set; } = string.Empty;

        // Stores album name the track belongs to
        public string AlbumName { get; set; } = string.Empty;

        // Stores album artwork URL
        public string AlbumImageUrl { get; set; } = string.Empty;

        // Stores Spotify link for the track
        public string SpotifyUrl { get; set; } = string.Empty;

        // Stores release date of the track
        public string ReleaseDate { get; set; } = string.Empty;

        // Stores ID of user who saved the track
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Stores date and time the track was saved
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}