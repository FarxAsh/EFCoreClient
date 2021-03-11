using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreClient.Data.Entities;

namespace EFCoreClient.Data.EntityTypeConfig
{
    class UserEntityTypeConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasIndex(e => e.Email, "AK_User_Email")
                .IsUnique();

            builder.HasIndex(e => e.PhoneNumber, "AK_User_PhoneNumber")
                .IsUnique();

            builder.Property(e => e.BuildingNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.FlatNumber).HasMaxLength(20);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(80);
        }
    }
}
