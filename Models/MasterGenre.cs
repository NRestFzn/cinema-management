using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class MasterGenre : BaseModel
    {
        public required string Name { get; set; }
        public ICollection<MasterMovie> MasterMovies { get; set; } = [];
    }

    public class MasterGenreConfiguration : IEntityTypeConfiguration<MasterGenre>
    {
        public void Configure(EntityTypeBuilder<MasterGenre> builder)
        {
            builder.ToTable("MasterGenres");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.HasIndex(e => e.Name).IsUnique(true);
            builder.Property(e => e.Name).HasColumnName("name").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasMany(e => e.MasterMovies)
                   .WithOne(e => e.MasterGenre)
                   .HasForeignKey(e => e.MasterGenreId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}