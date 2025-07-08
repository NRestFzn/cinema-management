using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Data
{
    public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
    {
        public DbSet<Chair> Chair { get; set; }
        public DbSet<Cinema> Cinema { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<MasterChairType> MasterChairType { get; set; }
        public DbSet<MasterGenre> MasterGenre { get; set; }
        public DbSet<MasterMovie> MasterMovie { get; set; }
        public DbSet<MasterStudioType> MasterStudioType { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<OperatingHour> OperatingHour { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Studio> Studio { get; set; }
        public DbSet<StudioFacility> StudioFacility { get; set; }
        public DbSet<User> User { get; set; }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseModel && (x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                ((BaseModel)entity.Entity).UpdatedAt = now;
            }
        }
    }
}