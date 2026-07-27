using GymSystem.BusinessLogic.Extensions;

namespace GymSystem.BusinessLogic.Services;

public class TrainerService : ITrainerService
{
    private readonly IGenericRepository<Trainer> _trainerRepo;
    private readonly IGenericRepository<Session> _sessionRepo;

    public TrainerService(
        IGenericRepository<Trainer> trainerRepo,
        IGenericRepository<Session> sessionRepo)
    {
        _trainerRepo = trainerRepo;
        _sessionRepo = sessionRepo;
    }

    public async Task<IEnumerable<TrainerViewModel>> GetTrainersAsync(CancellationToken cancellationToken = default)
    {
        var trainers = await _trainerRepo.GetAllAsync(cancellationToken: cancellationToken);

        return trainers.Select(t => new TrainerViewModel
        {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Phone = t.Phone,
            Specialization = t.Specialty.ToDisplayName()
        });
    }

    public async Task<bool> CreateTrainerAsync(
        CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default)
    {
        // Email and phone must be unique across trainers.
        var emailExists = await _trainerRepo
            .AnyAsync(t => t.Email == createTrainerViewModel.Email, cancellationToken);

        var phoneExists = await _trainerRepo
            .AnyAsync(t => t.Phone == createTrainerViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return false;

        var newTrainer = new Trainer
        {
            Name = createTrainerViewModel.Name,
            Email = createTrainerViewModel.Email,
            Phone = createTrainerViewModel.Phone,
            DateOfBirth = createTrainerViewModel.DateOfBirth,
            Gender = createTrainerViewModel.Gender,
            Specialty = createTrainerViewModel.Specialty,
            HireDate = DateTime.Now,
            Address = new Address
            {
                BuildingNumber = createTrainerViewModel.BuildingNumber,
                Street = createTrainerViewModel.Street,
                City = createTrainerViewModel.City
            }
        };

        return (await _trainerRepo.AddAsync(newTrainer, cancellationToken)) > 0;
    }

    public async Task<TrainerDetailsViewModel?> GetTrainerDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainer = await _trainerRepo.GetByIdAsync(id, cancellationToken);

        if (trainer is null)
            return null;

        return new TrainerDetailsViewModel
        {
            Id = trainer.Id,
            Name = trainer.Name,
            Specialization = $"{trainer.Specialty.ToDisplayName()} Trainer",
            Email = trainer.Email,
            Phone = trainer.Phone,
            DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
            Gender = trainer.Gender.ToString(),
            BuildingNumber = trainer.Address.BuildingNumber,
            Street = trainer.Address.Street,
            City = trainer.Address.City
        };
    }

    public async Task<EditTrainerViewModel?> GetTrainerForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainer = await _trainerRepo.GetByIdAsync(id, cancellationToken);

        if (trainer is null)
            return null;

        return new EditTrainerViewModel
        {
            Id = trainer.Id,
            Name = trainer.Name,
            DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
            Gender = trainer.Gender.ToString(),
            Email = trainer.Email,
            Phone = trainer.Phone,
            BuildingNumber = trainer.Address.BuildingNumber,
            Street = trainer.Address.Street,
            City = trainer.Address.City,
            Specialty = trainer.Specialty
        };
    }

    public async Task<bool> UpdateTrainerAsync(
        int id, EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default)
    {
        var trainer = await _trainerRepo.GetByIdAsync(id, cancellationToken);

        if (trainer is null)
            return false;

        // Email/Phone must stay unique across OTHER trainers.
        var emailExists = await _trainerRepo
            .AnyAsync(t => t.Id != id && t.Email == editTrainerViewModel.Email, cancellationToken);

        var phoneExists = await _trainerRepo
            .AnyAsync(t => t.Id != id && t.Phone == editTrainerViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return false;

        // Name, DateOfBirth and Gender are locked on the edit form and stay untouched.
        trainer.Email = editTrainerViewModel.Email;
        trainer.Phone = editTrainerViewModel.Phone;
        trainer.Specialty = editTrainerViewModel.Specialty;
        trainer.Address.BuildingNumber = editTrainerViewModel.BuildingNumber;
        trainer.Address.Street = editTrainerViewModel.Street;
        trainer.Address.City = editTrainerViewModel.City;

        return (await _trainerRepo.UpdateAsync(trainer, cancellationToken)) > 0;
    }

    public async Task<bool> HasScheduledSessionsAsync(int id, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;

        // A session still counts as scheduled until it has finished.
        return await _sessionRepo
            .AnyAsync(s => s.TrainerId == id && s.EndDate >= now, cancellationToken);
    }

    public async Task<DeleteTrainerResult> DeleteTrainerAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainer = await _trainerRepo.GetByIdAsync(id, cancellationToken);

        if (trainer is null)
            return DeleteTrainerResult.NotFound;

        if (await HasScheduledSessionsAsync(id, cancellationToken))
            return DeleteTrainerResult.HasScheduledSessions;

        // Permanent, hard delete - trainers are not soft-deleted.
        return (await _trainerRepo.DeleteAsync(trainer, cancellationToken)) > 0
            ? DeleteTrainerResult.Success
            : DeleteTrainerResult.NotFound;
    }
}
