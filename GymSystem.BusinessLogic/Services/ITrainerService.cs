namespace GymSystem.BusinessLogic.Services;

public interface ITrainerService
{
    Task<IEnumerable<TrainerViewModel>> GetTrainersAsync(CancellationToken cancellationToken = default);
    Task<CreateTrainerViewModel> LoadLookupsAsync(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default);
    Task<EditTrainerViewModel> LoadLookupsAsync(EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default);
    Task<bool> CreateTrainerAsync(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default);
    Task<TrainerDetailsViewModel?> GetTrainerDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<EditTrainerViewModel?> GetTrainerForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> UpdateTrainerAsync(int id, EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default);
    Task<bool> HasScheduledSessionsAsync(int id, CancellationToken cancellationToken = default);
    Task<DeleteTrainerResult> DeleteTrainerAsync(int id, CancellationToken cancellationToken = default);
}

public enum DeleteTrainerResult
{
    Success,
    NotFound,
    HasScheduledSessions
}
