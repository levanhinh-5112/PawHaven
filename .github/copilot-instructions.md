# Copilot Instructions

## Project Guidelines
- Create/switch to the task-specific feature branch before implementing ticket changes.

## Pull Request Guidelines

### PR Title Format
`feat|fix|chore|refactor|docs: [TICKET-ID] Short description`

Examples:
- `feat: PAW-12 Add placeholder IPetRepository and register in DI`
- `fix: PAW-5 Resolve null reference in PetService`
- `chore: PAW-3 Update dependencies`

### PR Body Template
Use the following structure for all PRs:

```markdown
## Description
Brief summary of the changes and why they were made.

## Related Ticket
- Closes #TICKET-ID (e.g., Closes PAW-12)

## Type of Change
- [ ] New feature
- [ ] Bug fix
- [ ] Breaking change
- [ ] Documentation update
- [ ] Refactoring/Code cleanup

## Changes Made
- Bullet point 1
- Bullet point 2
- Bullet point 3

## Acceptance Criteria
- [x] Criterion 1
- [x] Criterion 2
- [x] Criterion 3

## Testing
How was this tested? (e.g., manual testing, unit tests added, etc.)

## Build Status
- [ ] Build passes: `dotnet build`
- [ ] No warnings or errors
- [ ] All tests pass (if applicable)

## Additional Notes
Any additional context or information for reviewers.
```

### Example PR for PAW-12
**Title:** `feat: PAW-12 Add placeholder IPetRepository and register in DI`

**Body:**
```markdown
## Description
Implemented the dependency injection wiring across all four layers (Domain → Application → Infrastructure → API) by creating a placeholder IPetRepository interface, its internal implementation, and DI extension methods. This establishes the layer structure before real features are added.

## Related Ticket
- Closes PAW-12

## Type of Change
- [x] New feature
- [ ] Bug fix
- [ ] Breaking change
- [ ] Documentation update
- [ ] Refactoring/Code cleanup

## Changes Made
- Created `IPetRepository` interface in `PawHaven.Application/Abstractions`
- Created internal `PetRepository` implementation in `PawHaven.Infrastructure/Repositories`
- Added `ApplicationServiceExtensions` with `AddApplication()` method in `PawHaven.Application/Extensions`
- Added `InfrastructureServiceExtensions` with `AddInfrastructure()` method in `PawHaven.Infrastructure/Extensions`
- Updated `Program.cs` to call both DI extension methods
- Added `Microsoft.Extensions.DependencyInjection.Abstractions` (v10.0.0) package to Application and Infrastructure projects
- Removed `.gitkeep` files from folders with actual content

## Acceptance Criteria
- [x] `IPetRepository` exists in `PawHaven.Application/Abstractions` (namespace `PawHaven.Application.Abstractions`)
- [x] `PetRepository` exists in `PawHaven.Infrastructure/Repositories` and is `internal`
- [x] `AddApplication()` and `AddInfrastructure()` exist as extension methods, and `Program.cs` calls both
- [x] `PawHaven.API` does not reference `PetRepository` directly
- [x] No `IPetService` and no `Services` folder
- [x] `PawHaven.Domain` still has no project or package references
- [x] `dotnet build` passes with no errors or warnings
- [x] Changes committed on `feature/PAW-12-placeholder-pet-repository`

## Testing
- Verified with `dotnet build` - no errors or warnings
- DI registration verified during build validation
- All layer references validated

## Build Status
- [x] Build passes: `dotnet build`
- [x] No warnings or errors
- [x] No tests applicable for this infrastructure setup

## Additional Notes
This is a foundation layer that establishes the DI pattern for future feature development. The placeholder repository will be replaced with real database operations in subsequent stories.
```