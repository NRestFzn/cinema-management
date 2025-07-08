using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class StudioFacility : BaseModel
    {
        public required string Name { get; set; }
        public required int StudioId { get; set; }
        public required Studio Studio { get; set; }
    }

    public class StudioFacilityConfiguration : IEntityTypeConfiguration<StudioFacility>
    {
        public void Configure(EntityTypeBuilder<StudioFacility> builder)
        {
            builder.ToTable("studioFacilities");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.Name).HasColumnName("name").IsRequired();

            builder.Property(e => e.StudioId).HasColumnName("StudioId").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.Studio)
                   .WithMany(e => e.StudioFacilities)
                   .HasForeignKey(e => e.StudioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}