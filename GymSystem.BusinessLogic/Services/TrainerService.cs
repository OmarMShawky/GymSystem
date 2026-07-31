using AutoMapper;
using System.Linq.Expressions;

namespace GymSystem.BusinessLogic.Services;

public class TrainerService(IUnitOfWork unitOfWork, IMapper mapper) : ITrainerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    private static readonly Expression<Func<Trainer, object>>[] _categoryInclude =
    [
        t => t.Category
    ];

    public async Task<IEnumerable<TrainerViewModel>> GetTrainersAsync(CancellationToken cancellationToken = default)
    {
        var trainers = await _unitOfWork.GetRepository<Trainer>()
            .GetAllWithIncludesAsync(_categoryInclude, cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
    }

    public async Task<CreateTrainerViewModel> LoadLookupsAsync(
        CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default)
    {
        createTrainerViewModel.Categories = await GetCategoryLookupsAsync(cancellationToken);
        return createTrainerViewModel;
    }

    public async Task<EditTrainerViewModel> LoadLookupsAsync(
        EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default)
    {
        editTrainerViewModel.Categories = await GetCategoryLookupsAsync(cancellationToken);
        return editTrainerViewModel;
    }

    public async Task<Result> CreateTrainerAsync(
        CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default)
    {
        var trainerRepo = _unitOfWork.GetRepository<Trainer>();

        var emailExists = await trainerRepo
            .AnyAsync(t => t.Email == createTrainerViewModel.Email, cancellationToken);

        var phoneExists = await trainerRepo
            .AnyAsync(t => t.Phone == createTrainerViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return Result.Conflict("Email or phone already exists.");

        var newTrainer = _mapper.Map<Trainer>(createTrainerViewModel);

        trainerRepo.Add(newTrainer, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The trainer could not be saved.");
    }

    public async Task<Result<TrainerDetailsViewModel>> GetTrainerDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainer = await _unitOfWork.GetRepository<Trainer>()
            .GetByIdWithIncludesAsync(id, _categoryInclude, cancellationToken);

        if (trainer is null)
            return Result.NotFound<TrainerDetailsViewModel>("Trainer not found.");

        return _mapper.Map<TrainerDetailsViewModel>(trainer);
    }

    public async Task<Result<EditTrainerViewModel>> GetTrainerForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainer = await _unitOfWork.GetRepository<Trainer>()
            .GetByIdAsync(id, cancellationToken);

        if (trainer is null)
            return Result.NotFound<EditTrainerViewModel>("Trainer not found.");

        return await LoadLookupsAsync(_mapper.Map<EditTrainerViewModel>(trainer), cancellationToken);
    }

    public async Task<Result> UpdateTrainerAsync(
        int id, EditTrainerViewModel editTrainerViewModel, CancellationToken cancellationToken = default)
    {
        var trainerRepo = _unitOfWork.GetRepository<Trainer>();

        var trainer = await trainerRepo.GetByIdAsync(id, cancellationToken);

        if (trainer is null)
            return Result.NotFound("Trainer not found.");

        var emailExists = await trainerRepo
            .AnyAsync(t => t.Id != id && t.Email == editTrainerViewModel.Email, cancellationToken);

        var phoneExists = await trainerRepo
            .AnyAsync(t => t.Id != id && t.Phone == editTrainerViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return Result.Conflict("Email or phone is already in use by another trainer.");

        _mapper.Map(editTrainerViewModel, trainer);

        trainerRepo.Update(trainer, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The trainer could not be updated.");
    }

    public async Task<bool> HasScheduledSessionsAsync(int id, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;

        return await _unitOfWork.GetRepository<Session>()
            .AnyAsync(s => s.TrainerId == id && s.EndDate >= now, cancellationToken);
    }

    public async Task<Result> DeleteTrainerAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainerRepo = _unitOfWork.GetRepository<Trainer>();

        var trainer = await trainerRepo.GetByIdAsync(id, cancellationToken);

        if (trainer is null)
            return Result.NotFound("Trainer not found.");

        var hasAnySession = await _unitOfWork.GetRepository<Session>()
            .AnyAsync(s => s.TrainerId == id, cancellationToken);

        if (hasAnySession)
            return Result.Conflict("This trainer has scheduled sessions and cannot be deleted.");

        trainerRepo.Delete(trainer, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The trainer could not be deleted.");
    }

    private async Task<IEnumerable<LookupItemViewModel>> GetCategoryLookupsAsync(CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.GetRepository<Category>()
            .GetAllAsync(cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<LookupItemViewModel>>(categories.OrderBy(c => c.Name));
    }
}
