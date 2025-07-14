using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaManagement.Models
{
    public class TransactionDetail : BaseModel
    {
        public int TransactionId { get; set; }
        public int ChairId { get; set; }
        public decimal Price { get; set; }
        public Transaction Transaction { get; set; }
    }

    public class TransactionDetailConfiguration : IEntityTypeConfiguration<TransactionDetail>
    {
        public void Configure(EntityTypeBuilder<TransactionDetail> builder)
        {
            builder.ToTable("transactionDetails");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.TransactionId).HasColumnName("TransactionId").IsRequired();

            builder.Property(t => t.ChairId).HasColumnName("ChairId").IsRequired();

            builder.Property(t => t.Price).HasColumnName("price").HasColumnType("decimal(10, 2)").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.Transaction)
                .WithMany(e => e.TransactionDetail)
                .HasForeignKey(e => e.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}