using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class MasterStudioType : BaseModel
    {
        public required string Name { get; set; }
        public ICollection<Studio> Studios { get; set; } = [];
    }

    public class MasterStudioTypeConfiguration : IEntityTypeConfiguration<MasterStudioType>
    {
        public void Configure(EntityTypeBuilder<MasterStudioType> builder)
        {
            builder.ToTable("MasterStudioTypes");

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

            builder.HasMany(e => e.Studios)
                   .WithOne(e => e.StudioType)
                   .HasForeignKey(e => e.StudioTypeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}