using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Models
{
    public class Voucher : BaseModel
    {
        public required string Code { get; set; }
        public required int Discount { get; set; }
        public required DateTime ValidDate { get; set; }
        public required DateTime ExpiredDate { get; set; }
        public required int Quota { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }

    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("vouchers");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.Code).HasColumnName("code").IsRequired();

            builder.Property(e => e.Discount).HasColumnName("discount").IsRequired();

            builder.Property(e => e.ValidDate).HasColumnName("validDate").IsRequired();

            builder.Property(e => e.ExpiredDate).HasColumnName("expiredDate").IsRequired();

            builder.Property(e => e.Quota).HasColumnName("quota").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasMany(e => e.Transactions)
                   .WithOne(e => e.Voucher)
                   .HasForeignKey(e => e.VoucherId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}