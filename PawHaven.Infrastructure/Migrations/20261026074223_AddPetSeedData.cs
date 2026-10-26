using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PawHaven.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPetSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Species = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AgeInMonths = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdoptionApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdoptionApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdoptionApplications_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdoptionApplications_Users_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "AgeInMonths", "Breed", "CreatedAtUtc", "Description", "Name", "PhotoUrl", "Species", "Status" },
                values: new object[,]
                {
                    { 1, 24, "Golden Retriever", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A friendly Golden Retriever who loves to play fetch and swim.", "Max", null, "Dog", "Available" },
                    { 2, 36, "Labrador Retriever", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A playful Labrador Retriever with a gentle temperament, great with families.", "Bella", null, "Dog", "Available" },
                    { 3, 48, "German Shepherd", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An energetic German Shepherd who is loyal and protective of his family.", "Charlie", null, "Dog", "Pending" },
                    { 4, 30, "French Bulldog", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A cute French Bulldog with a calm personality, perfect for apartment living.", "Daisy", null, "Dog", "Adopted" },
                    { 5, 18, "Persian", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A beautiful white Persian cat with striking blue eyes, loves cuddling on the couch.", "Luna", null, "Cat", "Available" },
                    { 6, 24, "Siamese", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A sleek Siamese cat with distinctive markings, very vocal and affectionate.", "Whiskers", null, "Cat", "Available" },
                    { 7, 42, "Maine Coon", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An orange tabby Maine Coon, large and gentle, enjoys interactive play.", "Tiger", null, "Cat", "Adopted" },
                    { 8, 12, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A soft and gentle rabbit with brown and white fur, enjoys hopping around in spacious areas.", "Hoppy", null, "Rabbit", "Available" },
                    { 9, 8, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A colorful green parakeet who loves to sing and interact with people.", "Tweety", null, "Bird", "Available" },
                    { 10, 6, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An adorable guinea pig with soft fur, enjoys fresh vegetables and social interaction.", "Pika", null, "Other", "Available" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_ApplicantId",
                table: "AdoptionApplications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_PetId",
                table: "AdoptionApplications",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdoptionApplications");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
