using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class City : BaseModel
    {
        public required string Name { get; set; }
        public int ProvinceId { get; set; }
        public required Province Province { get; set; }

        public ICollection<Cinema> Cinemas = [];
    }

    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("cities");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.Name).HasColumnName("name").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.Province)
                   .WithMany(e => e.Cities)
                   .HasForeignKey(e => e.ProvinceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Cinemas)
                   .WithOne(e => e.City)
                   .HasForeignKey(e => e.CityId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}