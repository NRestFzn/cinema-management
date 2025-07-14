using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public enum ChairStatus
    {
        available,
        ordered,
        damaged
    }
    public class Chair : BaseModel
    {
        public required string ChairNumber { get; set; }
        public required ChairStatus ChairStatus { get; set; }
        public int StudioId { get; set; }
        public int ChairTypeId { get; set; }
        public required Studio Studio { get; set; }
        public required MasterChairType ChairType { get; set; }
    }

    public class ChairConfiguration : IEntityTypeConfiguration<Chair>
    {
        public void Configure(EntityTypeBuilder<Chair> builder)
        {
            builder.ToTable("chairs");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.ChairNumber).HasColumnName("chairNumber").IsRequired();

            builder.Property(e => e.ChairStatus).HasColumnName("chairStatus")
                                                .IsRequired()
                                                .HasConversion<string>()
                                                .HasColumnType("enum('available', 'ordered', 'damaged')");

            builder.Property(e => e.StudioId).HasColumnName("StudioId").IsRequired();

            builder.Property(e => e.ChairTypeId).HasColumnName("ChairTypeId").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.ChairType)
                   .WithMany(e => e.Chairs)
                   .HasForeignKey(e => e.ChairTypeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}