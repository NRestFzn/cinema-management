using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public enum OperatingDays
    {
        monday,
        tuesday,
        wednesday,
        thursday,
        friday,
        saturday,
        sunday
    }
    public class OperatingHour : BaseModel
    {
        public required OperatingDays Day { get; set; }
        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }
        public bool IsClose { get; set; }
        public int CinemaId { get; set; }
        public required Cinema Cinema { get; set; }
    }

    public class OperatingHourConfiguration : IEntityTypeConfiguration<OperatingHour>
    {
        public void Configure(EntityTypeBuilder<OperatingHour> builder)
        {
            builder.ToTable("operatingHours");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.Day).HasColumnName("day")
                                        .IsRequired()
                                        .HasConversion<string>()
                                        .HasColumnType("enum('monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday', 'sunday')");

            builder.Property(e => e.OpenTime).HasColumnName("closeTime").IsRequired();

            builder.Property(e => e.CloseTime).HasColumnName("openTime").IsRequired();

            builder.Property(e => e.IsClose).HasColumnName("isClose").IsRequired();

            builder.Property(e => e.CinemaId).HasColumnName("CinemaId").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.Cinema)
                   .WithMany(e => e.OperatingHours)
                   .HasForeignKey(e => e.CinemaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}