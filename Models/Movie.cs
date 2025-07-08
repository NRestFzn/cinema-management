using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public enum MovieStatus
    {
        upcoming,
        nowshowing
    }
    public class Movie : BaseModel
    {
        public int MasterMovieId { get; set; }
        public int CinemaId { get; set; }
        public MovieStatus Status { get; set; }
        public required MasterMovie MasterMovie { get; set; }
        public required Cinema Cinema { get; set; }
    }

    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("movies");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.MasterMovieId).HasColumnName("MasterMovieId").IsRequired();

            builder.Property(e => e.CinemaId).HasColumnName("CinemaId").IsRequired();

            builder.Property(e => e.Status).HasColumnName("status")
                                           .IsRequired()
                                           .HasConversion<string>()
                                           .HasColumnType("enum('upcoming', 'nowshowing')");

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.MasterMovie)
                   .WithMany(e => e.Movies)
                   .HasForeignKey(e => e.MasterMovieId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}