using PawHaven.Domain.Entities;
using PawHaven.Domain.Enums;

namespace PawHaven.Infrastructure.Persistence.Seed;

public static class PetSeedData
{
    private static readonly DateTime SeedDate = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static IReadOnlyList<Pet> Pets => new List<Pet>
    {
        // Dogs (4)
        new Pet("Max", Species.Dog, "A friendly Golden Retriever who loves to play fetch and swim.", ageInMonths: 24, status: PetStatus.Available, breed: "Golden Retriever", photoUrl: null, createdAtUtc: SeedDate),
        new Pet("Bella", Species.Dog, "A playful Labrador Retriever with a gentle temperament, great with families.", ageInMonths: 36, status: PetStatus.Available, breed: "Labrador Retriever", photoUrl: null, createdAtUtc: SeedDate),
        new Pet("Charlie", Species.Dog, "An energetic German Shepherd who is loyal and protective of his family.", ageInMonths: 48, status: PetStatus.Pending, breed: "German Shepherd", photoUrl: null, createdAtUtc: SeedDate),
        new Pet("Daisy", Species.Dog, "A cute French Bulldog with a calm personality, perfect for apartment living.", ageInMonths: 30, status: PetStatus.Adopted, breed: "French Bulldog", photoUrl: null, createdAtUtc: SeedDate),

        // Cats (3)
        new Pet("Luna", Species.Cat, "A beautiful white Persian cat with striking blue eyes, loves cuddling on the couch.", ageInMonths: 18, status: PetStatus.Available, breed: "Persian", photoUrl: null, createdAtUtc: SeedDate),
        new Pet("Whiskers", Species.Cat, "A sleek Siamese cat with distinctive markings, very vocal and affectionate.", ageInMonths: 24, status: PetStatus.Available, breed: "Siamese", photoUrl: null, createdAtUtc: SeedDate),
        new Pet("Tiger", Species.Cat, "An orange tabby Maine Coon, large and gentle, enjoys interactive play.", ageInMonths: 42, status: PetStatus.Adopted, breed: "Maine Coon", photoUrl: null, createdAtUtc: SeedDate),

        // Rabbit (1)
        new Pet("Hoppy", Species.Rabbit, "A soft and gentle rabbit with brown and white fur, enjoys hopping around in spacious areas.", ageInMonths: 12, status: PetStatus.Available, breed: null, photoUrl: null, createdAtUtc: SeedDate),

        // Bird (1)
        new Pet("Tweety", Species.Bird, "A colorful green parakeet who loves to sing and interact with people.", ageInMonths: 8, status: PetStatus.Available, breed: null, photoUrl: null, createdAtUtc: SeedDate),

        // Other (1)
        new Pet("Pika", Species.Other, "An adorable guinea pig with soft fur, enjoys fresh vegetables and social interaction.", ageInMonths: 6, status: PetStatus.Available, breed: null, photoUrl: null, createdAtUtc: SeedDate),
    };
}
