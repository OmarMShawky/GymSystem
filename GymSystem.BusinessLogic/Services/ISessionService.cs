namespace GymSystem.BusinessLogic.Services;

public interface ISessionService
{
    Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken cancellationToken = default);

    Task<CreateSessionViewModel> LoadLookupsAsync(
        CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default);

    Task<CreateSessionResult> CreateSessionAsync(
        CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default);
}

public enum CreateSessionResult
{
    Success,
    CategoryNotFound,
    TrainerNotFound,
    SpecialtyMismatch,
    TrainerBusy,
    SaveFailed
}
