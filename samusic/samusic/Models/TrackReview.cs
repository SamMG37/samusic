using System.ComponentModel.DataAnnotations;

namespace samusic.Models
{
    public class TrackReview
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

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string UserEmail { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}