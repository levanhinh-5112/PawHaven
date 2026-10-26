using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PawHaven.Domain.Entities;

namespace PawHaven.Infrastructure.Persistence.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(pet => pet.Id);

        builder.Property(pet => pet.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(pet => pet.Species)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(pet => pet.Breed)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(pet => pet.AgeInMonths)
            .IsRequired();

        builder.Property(pet => pet.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(pet => pet.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(pet => pet.PhotoUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(pet => pet.CreatedAtUtc)
            .IsRequired();
    }
}
