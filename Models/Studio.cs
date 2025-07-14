using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class Studio : BaseModel
    {
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public int StudioTypeId { get; set; }
        public int CinemaId { get; set; }
        public required MasterStudioType StudioType { get; set; }
        public required Cinema Cinema { get; set; }
        public ICollection<StudioFacility> StudioFacilities { get; set; } = [];
        public ICollection<MovieSchedule> MovieSchedules { get; set; } = [];
    }

    public class StudioConfiguration : IEntityTypeConfiguration<Studio>
    {
        public void Configure(EntityTypeBuilder<Studio> builder)
        {
            builder.ToTable("studios");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.Name).HasColumnName("name").IsRequired();

            builder.Property(e => e.Capacity).HasColumnName("capacity").IsRequired();

            builder.Property(e => e.StudioTypeId).HasColumnName("StudioTypeId").IsRequired()
            ;
            builder.Property(e => e.CinemaId).HasColumnName("CinemaId").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasMany(e => e.StudioFacilities)
                   .WithOne(e => e.Studio)
                   .HasForeignKey(e => e.StudioId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.StudioType)
                   .WithMany(e => e.Studios)
                   .HasForeignKey(e => e.StudioTypeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Cinema)
                   .WithMany(e => e.Studios)
                   .HasForeignKey(e => e.CinemaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.MovieSchedules)
                   .WithOne(e => e.Studio)
                   .HasForeignKey(e => e.StudioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}