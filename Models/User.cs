using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace CinemaManagement.Models
{
    public class User : BaseModel
    {
        public required string Fullname { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required int RoleId { get; set; }
        public required Role Role { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").IsRequired();

            builder.Property(e => e.Fullname).HasColumnName("fullname").IsRequired();

            builder.HasIndex(e => e.Email).IsUnique(true);
            builder.Property(e => e.Email).HasColumnName("email").IsRequired();

            builder.HasIndex(e => e.PhoneNumber).IsUnique(true);
            builder.Property(e => e.PhoneNumber).HasColumnName("phoneNumber").IsRequired();

            builder.Property(e => e.Password).HasColumnName("password").IsRequired();

            builder.Property(e => e.RoleId).HasColumnName("RoleId").IsRequired();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("createdAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updatedAt")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.HasOne(e => e.Role)
                   .WithMany(e => e.Users)
                   .HasForeignKey(e => e.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}