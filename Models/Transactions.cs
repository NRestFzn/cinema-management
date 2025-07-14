using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaManagement.Models
{
    public enum TransactionStatus
    {
        paid,
        unpaid,
        canceled
    }
    public class Transaction : BaseModel
    {
        public int UserId { get; set; }
        public int PaymentMethodId { get; set; }
        public int ScheduleId { get; set; }
        public int? VoucherId { get; set; }
        public required string TransactionCode { get; set; }
        public TransactionStatus Status { get; set; }
        // public int MembershipDiscount { get; set; }
        public int VoucherDiscount { get; set; }
        public int TotalDiscount { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public User User { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public MovieSchedule MovieSchedule { get; set; }
        public Voucher? Voucher { get; set; }
        public ICollection<TransactionDetail> TransactionDetail { get; set; } = [];
    }

    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(t => t.Id);

            builder.HasIndex(t => t.TransactionCode).IsUnique();
            builder.Property(t => t.TransactionCode)
                .IsRequired()
                .HasMaxLength(6);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnType("enum('paid','unpaid','canceled')");

            // builder.Property(t => t.MembershipDiscount).IsRequired().HasDefaultValue(0);
            builder.Property(t => t.VoucherDiscount).IsRequired().HasDefaultValue(0);
            builder.Property(t => t.TotalDiscount).IsRequired().HasDefaultValue(0);

            builder.Property(t => t.OriginalAmount).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(t => t.TotalAmount).IsRequired().HasColumnType("decimal(10,2)");

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.User)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PaymentMethod)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.MovieSchedule)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Voucher)
                .WithMany(e => e.Transactions)
                .HasForeignKey(t => t.VoucherId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.TransactionDetail)
               .WithOne(e => e.Transaction)
               .HasForeignKey(t => t.TransactionId)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}