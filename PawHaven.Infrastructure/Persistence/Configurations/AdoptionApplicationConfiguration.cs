using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PawHaven.Domain.Entities;

namespace PawHaven.Infrastructure.Persistence.Configurations;

public class AdoptionApplicationConfiguration : IEntityTypeConfiguration<AdoptionApplication>
{
    public void Configure(EntityTypeBuilder<AdoptionApplication> builder)
    {
        builder.HasKey(application => application.Id);

        builder.Property(application => application.Message)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(application => application.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(application => application.CreatedAtUtc)
            .IsRequired();

        builder.Property(application => application.ReviewedAtUtc)
            .IsRequired(false);

        builder.HasOne<Pet>()
            .WithMany()
            .HasForeignKey(application => application.PetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(application => application.ApplicantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
