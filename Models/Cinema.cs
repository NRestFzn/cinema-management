using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class Cinema : BaseModel
    {
        public required string Name { get; set; }
        public int CityId { get; set; }
        public required string DetailLocation { get; set; }
        public required City City { get; set; }
        public ICollection<OperatingHour> OperatingHours = [];
    }

    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.ToTable("cinemas");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.Name).HasColumnName("name").IsRequired();

            builder.Property(e => e.CityId).HasColumnName("CityId").IsRequired();

            builder.Property(e => e.DetailLocation).HasColumnName("detailLocation").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.City)
                   .WithMany(e => e.Cinemas)
                   .HasForeignKey(e => e.CityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.OperatingHours)
                   .WithOne(e => e.Cinema)
                   .HasForeignKey(e => e.CinemaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}