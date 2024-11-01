using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using MasjidApi.Models;

namespace MasjidApi.Data
{
    public class MasjidDBContext : DbContext
    {
        public MasjidDBContext(DbContextOptions<MasjidDBContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> UserInfo { get; set; } = null!;
        public DbSet<MasjidInfo> MasjidInfo { get; set; }
        //public DbSet<MasjidInfoDTO> MasjidInfoDTO { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<FavoriteMasjidsByUser> FavoriteMasjidsByUser { get; set; }
        public DbSet<MasjidPrayerTime> MasjidPrayerTime { get; set; }
        public DbSet<MasjidsEvent> MasjidsEvent { get; set; }
        public DbSet<EventTypes> EventTypes { get; set; }

        public DbSet<POSSubscription> POSSubscription { get; set; }
        public DbSet<Logging> Logging { get; set; }

        public DbSet<LudoSession> LudoSession { get; set; }
        public DbSet<LudoPlayingState> LudoPlayingState { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>()
                .HasKey(c => new { c.UserId });

            modelBuilder.Entity<MasjidInfo>()
                .HasKey(c => new { c.MasjidID });

        }




    }
}