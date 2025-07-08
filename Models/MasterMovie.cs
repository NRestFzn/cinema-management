using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class MasterMovie : BaseModel
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int MasterGenreId { get; set; }
        public required int Duration { get; set; }
        public required int AgeRating { get; set; }
        public required MasterGenre MasterGenre { get; set; }
        public ICollection<Movie> Movies { get; set; } = [];
    }

    public class MasterMovieConfiguration : IEntityTypeConfiguration<MasterMovie>
    {
        public void Configure(EntityTypeBuilder<MasterMovie> builder)
        {
            builder.ToTable("MasterMovies");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.Title).HasColumnName("title").IsRequired();

            builder.Property(e => e.Description).HasColumnName("description").IsRequired();

            builder.Property(e => e.MasterGenreId).HasColumnName("MasterGenreId").IsRequired();

            builder.Property(e => e.Duration).HasColumnName("duration").IsRequired();

            builder.Property(e => e.AgeRating).HasColumnName("ageRating").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.MasterGenre)
                   .WithMany(e => e.MasterMovies)
                   .HasForeignKey(e => e.MasterGenreId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Movies)
                   .WithOne(e => e.MasterMovie)
                   .HasForeignKey(e => e.MasterMovieId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}