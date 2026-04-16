namespace samusic.Models
{
    // Combines track details and review data for the reviews page
    public class TrackReviewsView
    {
        // Stores Spotify ID of the selected track
        public string SpotifyTrackId { get; set; } = string.Empty;

        // Stores name of the selected track
        public string TrackName { get; set; } = string.Empty;

        // Stores artist or artists linked to the track
        public string ArtistNames { get; set; } = string.Empty;

        // Stores album name the track belongs to
        public string AlbumName { get; set; } = string.Empty;

        // Stores album artwork URL
        public string AlbumImageUrl { get; set; } = string.Empty;

        // Stores Spotify link for the track
        public string SpotifyUrl { get; set; } = string.Empty;

        // Stores average rating for the track
        public double AverageRating { get; set; }

        // Stores total number of reviews for the track
        public int ReviewCount { get; set; }

        // Checks whether current user has already reviewed the track
        public bool HasReviewed { get; set; }

        // Stores all reviews linked to the track
        public List<TrackReview> Reviews { get; set; } = new();
    }
}