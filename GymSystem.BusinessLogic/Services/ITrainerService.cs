namespace GymSystem.BusinessLogic.Services;

public interface ITrainerService
{
    Task<IEnumerable<TrainerViewModel>> GetTrainersAsync(CancellationToken cancellationToken = default);
    Task<bool> CreateTrainerAsync(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default);
    Task<TrainerDetailsViewModel?> GetTrainerDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<EditTrainerViewModel?> GetTrainerForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> UpdateTrainerAsync(int id, EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default);

    /// <summary>True when the trainer still has a session that has not ended yet.</summary>
    Task<bool> HasScheduledSessionsAsync(int id, CancellationToken cancellationToken = default);

    Task<DeleteTrainerResult> DeleteTrainerAsync(int id, CancellationToken cancellationToken = default);
}

public enum DeleteTrainerResult
{
    Success,
    NotFound,
    HasScheduledSessions
}
