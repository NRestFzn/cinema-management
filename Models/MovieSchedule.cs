using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class MovieSchedule : BaseModel
    {
        public int MovieId { get; set; }
        public int StudioId { get; set; }
        public DateOnly ScreeningDate { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
        public required Movie Movie { get; set; }
        public required Studio Studio { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }

    public class MovieScheduleConfiguration : IEntityTypeConfiguration<MovieSchedule>
    {
        public void Configure(EntityTypeBuilder<MovieSchedule> builder)
        {
            builder.ToTable("MovieSchedules");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.MovieId).HasColumnName("MovieId").IsRequired();

            builder.Property(e => e.StudioId).HasColumnName("StudioId").IsRequired();

            builder.Property(e => e.ScreeningDate).HasColumnName("screeningDate").IsRequired();

            builder.Property(e => e.StartHour).HasColumnName("startHour").IsRequired();

            builder.Property(e => e.EndHour).HasColumnName("endHour").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.Movie)
                   .WithMany(e => e.MovieSchedules)
                   .HasForeignKey(e => e.MovieId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Studio)
                   .WithMany(e => e.MovieSchedules)
                   .HasForeignKey(e => e.MovieId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Transactions)
                   .WithOne(e => e.MovieSchedule)
                   .HasForeignKey(e => e.ScheduleId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}