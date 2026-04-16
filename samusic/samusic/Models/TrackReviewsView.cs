namespace samusic.Models
{
    public class TrackReviewsView
    {
        public string SpotifyTrackId { get; set; } = string.Empty;
        public string TrackName { get; set; } = string.Empty;
        public string ArtistNames { get; set; } = string.Empty;
        public string AlbumName { get; set; } = string.Empty;
        public string AlbumImageUrl { get; set; } = string.Empty;
        public string SpotifyUrl { get; set; } = string.Empty;

        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public bool HasReviewed { get; set; }

        public List<TrackReview> Reviews { get; set; } = new();
    }
}