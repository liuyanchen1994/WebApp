using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Community.Data.Entities;

namespace WebApp.Areas.Community.Data
{
    public class CommunityContext : DbContext
    {
        #region DBSet

        public DbSet<Blog> Blog { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<LikeAnalysis> LikeAnalysis { get; set; }

        #endregion
        public CommunityContext(DbContextOptions<CommunityContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Blog>().HasIndex(m => m.CatalogName);
            modelBuilder.Entity<Blog>().HasIndex(m => m.UpdatedTime);

            modelBuilder.Entity<LikeAnalysis>().HasIndex(m => m.ObjectId);
            modelBuilder.Entity<LikeAnalysis>().HasIndex(m => m.UpdatedTime);
            modelBuilder.Entity<LikeAnalysis>().HasIndex(m => m.ObjectType);


        }
    }
}
