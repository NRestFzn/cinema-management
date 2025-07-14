using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public enum DayTypeEnum
    {
        weekday, weekend
    }


    public class PriceRule : BaseModel
    {
        public required string Name { get; set; }
        public required int ChairTypeId { get; set; }
        public required int StudioId { get; set; }
        public DayTypeEnum DayType { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        public DateTime DaysDate { get; set; }
        public decimal Price { get; set; }
        public required MasterChairType ChairType { get; set; }
        public required Studio Studio { get; set; }
    }

    public class PriceRuleConfiguration : IEntityTypeConfiguration<PriceRule>
    {
        public void Configure(EntityTypeBuilder<PriceRule> builder)
        {
            builder.ToTable("priceRules");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.Name).HasColumnName("name").IsRequired();
            builder.Property(e => e.ChairTypeId).HasColumnName("ChairTypeId").IsRequired();
            builder.Property(e => e.StudioId).HasColumnName("StudioId").IsRequired();
            builder.Property(e => e.DayType).HasColumnName("dayType").IsRequired().HasColumnType("enum('weekend','weekday')"); ;
            builder.Property(e => e.IsWeekend).HasColumnName("isWeekend").IsRequired();
            builder.Property(e => e.IsHoliday).HasColumnName("isHoliday").IsRequired();
            builder.Property(e => e.DaysDate).HasColumnName("daysDate").IsRequired();
            builder.Property(e => e.Price).HasColumnName("price").IsRequired().HasColumnType("decimal(10, 2)");

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.Studio)
            .WithMany(e => e.PriceRules)
            .HasForeignKey(e => e.StudioId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.ChairType)
            .WithMany(e => e.PriceRules)
            .HasForeignKey(e => e.ChairTypeId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}