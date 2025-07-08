using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class MasterChairType : BaseModel
    {
        public required string Name { get; set; }
        public ICollection<Chair> Chairs { get; set; } = [];
    }

    public class MasterChairTypeConfiguration : IEntityTypeConfiguration<MasterChairType>
    {
        public void Configure(EntityTypeBuilder<MasterChairType> builder)
        {
            builder.ToTable("MasterChairTypes");

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

            builder.HasMany(e => e.Chairs)
                   .WithOne(e => e.ChairType)
                   .HasForeignKey(e => e.ChairTypeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}