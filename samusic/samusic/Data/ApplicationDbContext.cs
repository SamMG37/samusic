using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using samusic.Models;

namespace samusic.Data
{
    // Defines the database context used to access application data
    public class ApplicationDbContext : IdentityDbContext
    {
        // Passes database configuration options into the base Identity context
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Stores users' favourited songs
        public DbSet<FavouriteSong> FavouriteSongs { get; set; }

        // Stores user reviews and ratings for tracks
        public DbSet<TrackReview> TrackReviews { get; set; }
    }
}