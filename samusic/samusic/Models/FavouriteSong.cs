using System.ComponentModel.DataAnnotations;

namespace samusic.Models
{
    public class FavouriteSong
    {
        public int Id { get; set; }

        [Required]
        public string SpotifyTrackId { get; set; } = string.Empty;

        [Required]
        public string TrackName { get; set; } = string.Empty;

        [Required]
        public string ArtistNames { get; set; } = string.Empty;

        public string AlbumName { get; set; } = string.Empty;

        public string AlbumImageUrl { get; set; } = string.Empty;

        public string SpotifyUrl { get; set; } = string.Empty;

        public string ReleaseDate { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}