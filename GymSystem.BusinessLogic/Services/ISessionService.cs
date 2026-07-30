namespace GymSystem.BusinessLogic.Services;

public interface ISessionService
{
    // No failure mode - an empty list is a valid result.
    Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken cancellationToken = default);

    Task<CreateSessionViewModel> LoadLookupsAsync(
        CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default);

    Task<EditSessionViewModel> LoadLookupsAsync(
        EditSessionViewModel editSessionViewModel, CancellationToken cancellationToken = default);

    Task<Result> CreateSessionAsync(
        CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default);

    /// <summary>Read-only summary, also used by the delete confirmation page.</summary>
    Task<Result<SessionDetailsViewModel>> GetSessionDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<Result<EditSessionViewModel>> GetSessionForEditAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> UpdateSessionAsync(
        int id, EditSessionViewModel editSessionViewModel, CancellationToken cancellationToken = default);

    Task<Result> DeleteSessionAsync(int id, CancellationToken cancellationToken = default);
}
