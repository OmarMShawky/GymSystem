namespace GymSystem.BusinessLogic.Services;

public interface ITrainerService
{

    Task<IEnumerable<TrainerViewModel>> GetTrainersAsync(CancellationToken cancellationToken = default);

    Task<CreateTrainerViewModel> LoadLookupsAsync(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default);

    Task<EditTrainerViewModel> LoadLookupsAsync(EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default);

    Task<Result> CreateTrainerAsync(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default);
    Task<Result<TrainerDetailsViewModel>> GetTrainerDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<EditTrainerViewModel>> GetTrainerForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdateTrainerAsync(int id, EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default);

    Task<bool> HasScheduledSessionsAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> DeleteTrainerAsync(int id, CancellationToken cancellationToken = default);
}
