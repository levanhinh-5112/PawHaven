using PawHaven.Domain.Entities;
using PawHaven.Domain.Enums;

namespace PawHaven.Infrastructure.Persistence.Seed;

public static class PetSeedData
{
    private static readonly DateTime SeedDate = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static IReadOnlyList<Pet> Pets => new List<Pet>
    {
        // Dogs (4)
        Pet.CreateSeed(id: 1, name: "Max", species: Species.Dog, description: "A friendly Golden Retriever who loves to play fetch and swim.", ageInMonths: 24, status: PetStatus.Available, breed: "Golden Retriever", photoUrl: null, createdAtUtc: SeedDate),
        Pet.CreateSeed(id: 2, name: "Bella", species: Species.Dog, description: "A playful Labrador Retriever with a gentle temperament, great with families.", ageInMonths: 36, status: PetStatus.Available, breed: "Labrador Retriever", photoUrl: null, createdAtUtc: SeedDate),
        Pet.CreateSeed(id: 3, name: "Charlie", species: Species.Dog, description: "An energetic German Shepherd who is loyal and protective of his family.", ageInMonths: 48, status: PetStatus.Pending, breed: "German Shepherd", photoUrl: null, createdAtUtc: SeedDate),
        Pet.CreateSeed(id: 4, name: "Daisy", species: Species.Dog, description: "A cute French Bulldog with a calm personality, perfect for apartment living.", ageInMonths: 30, status: PetStatus.Adopted, breed: "French Bulldog", photoUrl: null, createdAtUtc: SeedDate),

        // Cats (3)
        Pet.CreateSeed(id: 5, name: "Luna", species: Species.Cat, description: "A beautiful white Persian cat with striking blue eyes, loves cuddling on the couch.", ageInMonths: 18, status: PetStatus.Available, breed: "Persian", photoUrl: null, createdAtUtc: SeedDate),
        Pet.CreateSeed(id: 6, name: "Whiskers", species: Species.Cat, description: "A sleek Siamese cat with distinctive markings, very vocal and affectionate.", ageInMonths: 24, status: PetStatus.Available, breed: "Siamese", photoUrl: null, createdAtUtc: SeedDate),
        Pet.CreateSeed(id: 7, name: "Tiger", species: Species.Cat, description: "An orange tabby Maine Coon, large and gentle, enjoys interactive play.", ageInMonths: 42, status: PetStatus.Adopted, breed: "Maine Coon", photoUrl: null, createdAtUtc: SeedDate),

        // Rabbit (1)
        Pet.CreateSeed(id: 8, name: "Hoppy", species: Species.Rabbit, description: "A soft and gentle rabbit with brown and white fur, enjoys hopping around in spacious areas.", ageInMonths: 12, status: PetStatus.Available, breed: null, photoUrl: null, createdAtUtc: SeedDate),

        // Bird (1)
        Pet.CreateSeed(id: 9, name: "Tweety", species: Species.Bird, description: "A colorful green parakeet who loves to sing and interact with people.", ageInMonths: 8, status: PetStatus.Available, breed: null, photoUrl: null, createdAtUtc: SeedDate),

        // Other (1)
        Pet.CreateSeed(id: 10, name: "Pika", species: Species.Other, description: "An adorable guinea pig with soft fur, enjoys fresh vegetables and social interaction.", ageInMonths: 6, status: PetStatus.Available, breed: null, photoUrl: null, createdAtUtc: SeedDate),
    };
}
