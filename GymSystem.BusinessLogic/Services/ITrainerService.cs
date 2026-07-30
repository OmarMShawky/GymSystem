namespace GymSystem.BusinessLogic.Services;

public interface ITrainerService
{
    // No failure mode - an empty list is a valid result.
    Task<IEnumerable<TrainerViewModel>> GetTrainersAsync(CancellationToken cancellationToken = default);

    /// <summary>Fills the Specialty (category) dropdown on the create form.</summary>
    Task<CreateTrainerViewModel> LoadLookupsAsync(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default);

    /// <summary>Fills the Specialty (category) dropdown on the edit form.</summary>
    Task<EditTrainerViewModel> LoadLookupsAsync(EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default);

    Task<Result> CreateTrainerAsync(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default);
    Task<Result<TrainerDetailsViewModel>> GetTrainerDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<EditTrainerViewModel>> GetTrainerForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdateTrainerAsync(int id, EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default);

    /// <summary>True when the trainer still has a session that has not ended yet.</summary>
    Task<bool> HasScheduledSessionsAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> DeleteTrainerAsync(int id, CancellationToken cancellationToken = default);
}
