namespace GymSystem.BusinessLogic.Services;

public interface ISessionService
{

    Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken cancellationToken = default);

    Task<CreateSessionViewModel> LoadLookupsAsync(
        CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default);

    Task<EditSessionViewModel> LoadLookupsAsync(
        EditSessionViewModel editSessionViewModel, CancellationToken cancellationToken = default);

    Task<Result> CreateSessionAsync(
        CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default);

    Task<Result<SessionDetailsViewModel>> GetSessionDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<Result<EditSessionViewModel>> GetSessionForEditAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> UpdateSessionAsync(
        int id, EditSessionViewModel editSessionViewModel, CancellationToken cancellationToken = default);

    Task<Result> DeleteSessionAsync(int id, CancellationToken cancellationToken = default);
}
