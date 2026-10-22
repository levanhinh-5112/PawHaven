using PawHaven.Domain.Enums;

namespace PawHaven.Domain.Entities;

public class AdoptionApplication
{
    public int Id { get; private set; }
    public int PetId { get; private set; }
    public int ApplicantId { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public ApplicationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? ReviewedAtUtc { get; private set; }

    // Private parameterless constructor for EF Core
    private AdoptionApplication()
    {
    }

    public AdoptionApplication(
        int petId,
        int applicantId,
        string message,
        ApplicationStatus status = ApplicationStatus.Pending,
        DateTime? createdAtUtc = null,
        DateTime? reviewedAtUtc = null)
    {
        if (petId <= 0)
            throw new ArgumentException("Pet ID must be greater than zero.", nameof(petId));

        if (applicantId <= 0)
            throw new ArgumentException("Applicant ID must be greater than zero.", nameof(applicantId));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.", nameof(message));

        if (message.Length > 2000)
            throw new ArgumentException("Message cannot exceed 2000 characters.", nameof(message));

        PetId = petId;
        ApplicantId = applicantId;
        Message = message;
        Status = status;
        CreatedAtUtc = createdAtUtc ?? DateTime.UtcNow;
        ReviewedAtUtc = reviewedAtUtc;
    }

    public void Approve(DateTime? reviewedAtUtc = null)
    {
        Status = ApplicationStatus.Approved;
        ReviewedAtUtc = reviewedAtUtc ?? DateTime.UtcNow;
    }

    public void Reject(DateTime? reviewedAtUtc = null)
    {
        Status = ApplicationStatus.Rejected;
        ReviewedAtUtc = reviewedAtUtc ?? DateTime.UtcNow;
    }
}
