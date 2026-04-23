using System.ComponentModel.DataAnnotations;

namespace samusic.Models
{
    // Stores user ratings and written reviews for tracks
    public class TrackReview
    {
        // Unique ID for each review entry
        public int Id { get; set; }

        // Stores Spotify ID of the reviewed track
        [Required]
        public string SpotifyTrackId { get; set; } = string.Empty;

        // Stores name of the reviewed track
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

        // Stores user's rating on a 1 to 5 scale
        [Range(1, 5)]
        public int Rating { get; set; }

        // Stores written review comment
        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;

        // Stores ID of the user who created the review
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Stores email of the user who created the review
        [Required]
        public string UserEmail { get; set; } = string.Empty;

        // Stores date and time the review was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
